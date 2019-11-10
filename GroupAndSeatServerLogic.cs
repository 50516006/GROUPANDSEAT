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
        /// <param name="args">引数 席の総数を示す10進数整数</param>
        static int Main(string[] args)
        {
            //引数確認
            if (args.Length != 1)
            {
                Console.WriteLine("引数の数がおかしいです");
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
            //待ち受け準備と開始
            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 9080);
            //外部でのプロセス監視用
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("/usr/local/bin/pidgass.txt", false);
                sw.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
                sw.Close();
            }
            catch { }

            //クエリ受信開始
            do
            {
                listener.Start();
                System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
                var ns = client.GetStream();
                var nsr = new System.IO.StreamReader(ns, System.Text.Encoding.UTF8);
                    
                ns.ReadTimeout = ns.WriteTimeout = 10000;
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                bool disconnected = false;
                //クエリ解釈して実行
                try
                {
                    string[] strs = nsr.ReadLine().Split(' ');
                    string ans;
                    //入店
                    if (strs[0] == "OPEN"　| strs[0]=="SET")
                        ans = Open(strs);
                    //退店時処理
                    else if (strs[0] == "CLOSE" | strs[0] == "REMOVE")
                        ans = strs.Length != 2 ? "ERR INVALIED_COMMANDLENGTH" : Gremove_From_Seat(int.Parse(strs[1]));
                    //グループ情報取得
                    else if (strs[0] == "GET")
                        ans = Get();
                    //席の使用情報
                    else if (strs[0] == "SC")
                        ans = Seat_Chec();
                    //席の使用情報識別付き
                    else if (strs[0] == "SSC")
                        ans = SuperSeatCheck();
                    //グループの時間書き換え
                    else if (strs[0] == "TC")
                        ans = TimeChange(strs[1],strs[2]);
                    //席移動
                    else if (strs[0] == "CHS")
                        ans = SeatChange(int.Parse(strs[1]), strs[2].Split(','));
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
        /// <param name="iden">席番の文字列型</param>
        /// <param name="time">現在時刻から何分前を開始時刻とするか</param>
        /// <returns>結果を示す文字列</returns>
        public static string TimeChange(string iden, string time) {
            
            int index;
            try {
                index = int.Parse(iden);
                if (seats[index] == null) {
                    return "SEAT_MUST_BE_USED";
                } 

            }
            catch {
                return "SEAT_NOMBER_MUST_BE_A_NOMBER";
            }
            try
            {
                int timen = int.Parse(time);
                System.DateTime now = System.DateTime.Now;
                seats[index].start = now.AddMinutes(-timen);
                return string.Format("OK_CHANGING_START_TIME_FOR_SEAT{0}_IS_ACCEPTED",iden);
            }
            catch {
                return "TIME_MUST_BE_INT";

            }




        }
        /// <summary>
        /// グループの使用する席を変更する
        /// </summary>
        /// <param name="iden">グループが使用している席のうち一つ</param>
        /// <param name="nss">グループが新たに使用する席の文字列</param>
        /// <returns>成否を示す文字列</returns>
        public static string SeatChange(int iden, string[] nss)
        {
            List<int> ns;
            try
            {
                ns = nss.Select(x => int.Parse(x)).ToList();
            }
            catch {
                    return "Seat must be nomber";
            }


            if (seats[iden] == null)
                return "Blank Group";
            Group g = seats[iden];
            if (ns.Any(x => seats[x] != null && seats[x] != g))
            {
                return "Other Group's Seat Cannot Be Selected";
            }
            g.seatnombers.ForEach(x => seats[x] = null);
            g.seatnombers = ns;
            ns.ForEach(x => seats[x] = g);
            return "Ok Changed";
        }
        /// <summary>
        /// 入店
        /// </summary>
        /// <param name="strs">配列へと変換された文字列</param>
        /// <returns>成否を示す文字列</returns>
        public static string Open(string[] strs )
        {
            //引数なしのコマンドにエラーを返す
            if (strs.Length < 2)
                return "ERR INVALIED_COMMAND_LENGTH";
            List<int> sn ;
            //使用席のリストを作成
            try
            {
                sn=strs.Skip(1).Select(x=>int.Parse(x)).ToList();
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
        /// <param name="l">使用予定席番</param>
        /// <returns>成否を示す文字列</returns>        
        public static string Gadd(List<int> l)
        {
            if(l.Any(x=>seats[x]!=null))
                return "ERR USED_SEAT_EXISTS";
            try
            {
                groups.Add(new Group(l, ref seats));
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
            int[] ss=Enumerable.Repeat(999, seats.Length).ToArray();
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].seatnombers.ForEach(x => ss[x] = i);
            }
            string ans = "OK " + string.Join(" ", ss);

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
            string ans = "OK";
            foreach (Group g in seats)
                ans += g == null ? " E" : " U";
            return ans;
        }
        /// <summary>
        /// 全グループの情報取得
        /// </summary>
        /// <returns>グループの情報を示す文字列</returns>
        public static string Get()
        {
            string ans = "OK " + groups.Count + " Groups Exists\r\n";
            ans+=string.Join("\r\n", groups.Select(x => x.Print_Stat()).ToList());
            return ans;
        }
    }
}












