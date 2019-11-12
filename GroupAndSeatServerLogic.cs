using System.Collections.Generic;
using System.Linq;
using System;
namespace GroupAndSeat
{
    /// <summary>
    /// サーバーの動作を記述してあるクラス
    /// </summary>
    /// <param name="groups">店内のグループを示すリスト</param>
    /// <param name="seats">席の番号から席を使用しているグループを探すための逆引きリスト</param>
    class ServerProcess
    {
        public static List<Group> groups;
        static Group[] seats;
        /// <summary>
        /// エントリーポイント
        /// </summary>
        /// <param name="args">引数 席の総数を示す10進数整数 ポート番号</param>
        static int Main(string[] args)
        {
            string argErro = "引数は席数とポート番号の二つです";
            //引数確認
            if (args.Length != 2)
            {
                Console.WriteLine(argErro);
                return 1;
            }
            //グループ保存領域の初期化
            groups = new List<Group>();
            try
            {
                seats = new Group[int.Parse(args[0])];
            }
            catch
            {
                Console.WriteLine("席数は非負整数値です");
                return 1;
            }
            int listeningPortNomber;
            try
            {
                listeningPortNomber = int.Parse(args[1]);
                if (listeningPortNomber > 65535 | listeningPortNomber <1024 )
                {
                    Console.WriteLine("自由に使用可能なポート番号にしてください");
                    return 1;
                }
            }
            catch
            {
                Console.WriteLine(argErro);
                return 1;
            }
            //待ち受け準備と開始
            
            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, listeningPortNomber);
            //外部でのプロセス監視用
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("/usr/local/bin/pidgass.txt", false);
                sw.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
                sw.Close();
            }
            catch { }

            listener.Start();
            //クエリ受信開始
            do
            {
                System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
                var ns = client.GetStream();
                var nsr = new System.IO.StreamReader(ns, System.Text.Encoding.UTF8);
                    
                ns.ReadTimeout = ns.WriteTimeout = 10000;
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                bool disconnected = false;
                //クエリ解釈して実行
                try
                {
                    string[] splitedQuery = nsr.ReadLine().Split(' ');
                    string ans;
                    //入店
                    if (splitedQuery[0] == "OPEN"　| splitedQuery[0]=="SET")
                        ans = Open(splitedQuery);
                    //退店時処理
                    else if (splitedQuery[0] == "CLOSE" | splitedQuery[0] == "REMOVE")
                        ans = splitedQuery.Length != 2 ? "ERR INVALIED_COMMANDLENGTH" : Gremove_From_Seat(int.Parse(splitedQuery[1]));
                    //グループ情報取得
                    else if (splitedQuery[0] == "GET")
                        ans = Get();
                    //席の使用情報
                    else if (splitedQuery[0] == "SC")
                        ans = Seat_Chec();
                    //席の使用情報識別付き
                    else if (splitedQuery[0] == "SSC")
                        ans = SuperSeatCheck();
                    //グループの時間書き換え
                    else if (splitedQuery[0] == "TC")
                        ans = TimeChange(splitedQuery[1],splitedQuery[2]);
                    //席移動
                    else if (splitedQuery[0] == "CHS")
                        ans = SeatChange(int.Parse(splitedQuery[1]), splitedQuery[2].Split(','));
                    //未実装コマンド
                    else
                        ans = "ERRO THERE_IS_NOT_SUCH_A_COMMAND";
                    if (!disconnected)
                        ns.Write(enc.GetBytes(ans + '\n'), 0, enc.GetBytes(ans + '\n').Length);
                }
                catch { }
                try
                {
                    ns.Close();
                    client.Close();
                } catch { }
            } while (true);
        }
        /// <summary>
        /// 開始時間変更
        /// </summary>
        /// <param name="index">席番の文字列型</param>
        /// <param name="minusMinutes_string">現在時刻から何分前を開始時刻とするか</param>
        /// <returns>結果を示す文字列</returns>
        public static string TimeChange(string groupIdentifireSeat, string minusMinutes_string) {
            
            int index;
            try {
                index = int.Parse(groupIdentifireSeat);
                if (seats[index] == null) {
                    return "SEAT_MUST_BE_USED";
                } 

            }
            catch {
                return "SEAT_NOMBER_MUST_BE_A_NOMBER";
            }
            try
            {
                int timen = int.Parse(minusMinutes_string);
                System.DateTime now = System.DateTime.Now;
                seats[index].start = now.AddMinutes(-timen);
                return string.Format("OK_CHANGING_START_TIME_FOR_SEAT{0}_IS_ACCEPTED",groupIdentifireSeat);
            }
            catch {
                return "TIME_MUST_BE_INT";

            }




        }
        /// <summary>
        /// グループの使用する席を変更する
        /// </summary>
        /// <param name="groupIdentifireSeat">グループが使用している席のうち一つ</param>
        /// <param name="newSeatsNombersString">グループが新たに使用する席の文字列</param>
        /// <returns>成否を示す文字列</returns>
        public static string SeatChange(int groupIdentifireSeat, string[] newSeatsNombersString)
        {
            List<int> newSeatNombers;
            try
            {
                newSeatNombers = newSeatsNombersString.Select(x => int.Parse(x)).ToList();
            }
            catch {
                    return "Seat must be nomber";
            }


            if (seats[groupIdentifireSeat] == null)
                return "Blank Group";
            Group g = seats[groupIdentifireSeat];
            if (newSeatNombers.Any(x => seats[x] != null && seats[x] != g))
            {
                return "Other Group's Seat Cannot Be Selected";
            }
            g.seatnombers.ForEach(x => seats[x] = null);
            g.seatnombers = newSeatNombers;
            newSeatNombers.ForEach(x => seats[x] = g);
            return "Ok Changed";
        }
        /// <summary>
        /// 入店
        /// </summary>
        /// <param name="splitedQuery">配列へと変換されたクエリ文字列</param>
        /// <returns>成否を示す文字列</returns>
        public static string Open(string[] splitedQuery )
        {
            //引数なしのコマンドにエラーを返す
            if (splitedQuery.Length < 2)
                return "ERR INVALIED_COMMAND_LENGTH";
            List<int> sn ;
            //使用席のリストを作成
            try
            {
                sn=splitedQuery.Skip(1).Select(x=>int.Parse(x)).ToList();
            }
            catch
            {
                return "ERR NONNOMBER IS INCLUDED";
            }

            return Gadd(sn);
        }
        /// <summary>
        /// 席を作成
        /// </summary>
        /// <param name="willUsedSeat">使用予定席番</param>
        /// <returns>成否を示す文字列</returns>        
        public static string Gadd(List<int> willUsedSeat)
        {
            if(willUsedSeat.Any(x=>seats[x]!=null))
                return "ERR USED_SEAT_EXISTS";
            try
            {
                groups.Add(new Group(willUsedSeat, ref seats));
                return "OK ONE_GROUP_HAS_BEEN_RESISTERED";
            }
            catch
            {
                return "ERR";
            }
        }
        /// <summary>
        /// グループ識別可能な席使用情報を作成
        /// </summary>
        /// <returns>席使用情報</returns>        
        public static string SuperSeatCheck()
        {
            int[] seatsStatus=Enumerable.Repeat(999, seats.Length).ToArray();
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].seatnombers.ForEach(x => seatsStatus[x] = i);
            }
            string ans = "OK " + string.Join(" ", seatsStatus);

            return ans;
        }
        /// <summary>
        /// グループを席番号から削除
        /// </summary>
        /// <param name="seatnomber">削除したいグループが使用している席一つ</param>
        /// <returns>成否を示す文字列</returns>
        public static string Gremove_From_Seat( int seatnomber)
        {
            if (seats[seatnomber] == null)
            {
                return "ERR THE_SEAT_IS_USED";
            }
            //席番を含むグループを選択し開放処理を行う
            try
            {
                Group g= groups.Where(x => x.seatnombers.Contains(seatnomber)).First();
                g.Rerease(ref seats);
                groups.Remove(g);
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

            return "OK GROUP_IS_REMOVED";
        }
        /// <summary>
        /// 識別情報なしの席使用情報を取得
        /// </summary>
        /// <returns>席使用情報を示す文字列</returns>
        public static string Seat_Chec()
        {
            return "OK " + string.Join(" ", seats.Select(x => x == null ? "E" : "U").ToArray());
        }
        /// <summary>
        /// 全グループの情報取得
        /// </summary>
        /// <returns>グループの情報を示す文字列</returns>
        public static string Get()
        {
            string ans = "OK " + groups.Count + " Groups Exists\r\n";
            return ans+string.Join("\r\n", groups.Select(x => x.Print_Stat()).ToList());
        }
    }
}












