using System.Collections.Generic;
namespace GroupAndSeat
{
    class Program
    {
        public static List<Group> groups;
        static Group[] seats;
        static void Main(string[] args)
        {
            groups = new List<Group>();
            seats = new Group[30];

            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 9080);
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("/usr/local/bin/pidgass.txt", false);
                sw.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
                sw.Close();
            }
            catch { }
            do
            {
                listener.Start();
                System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
                var ns = client.GetStream();
                var nsr = new System.IO.StreamReader(ns, System.Text.Encoding.UTF8);
                ns.ReadTimeout = ns.WriteTimeout = 10000;
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                bool disconnected = false;
                try
                {
                    string[] strs = nsr.ReadLine().Split(' ');
                    string ans;
                    if (strs[0] == "OPEN")
                        ans = Open(strs);
                    else if (strs[0] == "CLOSE")
                        ans = strs.Length != 2 ? "ERR INVALIED_COMMANDLENGTH" : Gremove_From_Seat(int.Parse(strs[1]));
                    else if (strs[0] == "GET")
                        ans = Get();
                    else if (strs[0] == "SC")
                        ans = Seat_Chec();
                    else if (strs[0] == "SSC")
                        ans = SuperSeatCheck();
                    else if (strs[0] == "TC")
                        ans = TimeChange(strs[1],strs[2]);
                    else if (strs[0] == "CHS")
                        ans = SeatChange(int.Parse(strs[1]), strs[2].Split(','));
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
        public static string TimeChange(string iden, string time) {
            int idn;
            try {
                 idn = int.Parse(iden);
                if (seats[idn] == null) {
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
                seats[idn].start = now.AddMinutes(-timen);
                return string.Format("OK_CHANGING_START_TIME_FOR_SEAT{0}_IS_ACCEPTED",iden);
            }
            catch {
                return "TIME_MUST_BE_INT";

            }




        }

        public static string SeatChange(int iden, string[] nss)
        {
            List<int> ns = new List<int>();
            foreach (string st in nss)
                try
                {
                    ns.Add(int.Parse(st));
                }
                catch {
                    return "Seat must be nomber";
                }
            if (seats[iden] == null)
                return "Blank Group";
            Group g = seats[iden];
            foreach (int i in ns)
                if ((seats[i] != null) && (seats[i] != g))
                    return "Other Group's Seat Cannot Be Selected";
            foreach (int j in g.seatnombers)
                seats[j] = null;
            g.seatnombers = ns;
            foreach (int k in ns)
                seats[k] = g;
            return "Ok Changed";
        }
        public static string Open(string[] strs )
        {
            if (strs.Length < 2)
                return "ERR INVALIED_COMMAND_LENGTH";
            List<int> sn = new List<int>();
            try
            {
                for (int i = 1; i < strs.Length; i++)
                    sn.Add(int.Parse(strs[i]));
            }
            catch
            {
                return "ERR NONNOMBER IS INCLUDED";
            }
            return Gadd(sn);
        }
        public static string Gadd(List<int> l)
        {
            foreach (int i in l)
            {
                if (seats[i] != null)
                {
                    return "ERR USED_SEAT_EXISTS";
                }
            }
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
        public static string SuperSeatCheck()
        {
            string ans = "OK";

            int[] ss = new int[30];
            for (int i = 0; i < 30; i++)
            {
                ss[i] = 999;

            }
            for (int i = 0; i < groups.Count; i++)
            {
                foreach (int j in groups[i].seatnombers)
                {
                    ss[j] = i;
                }
            }
            for (int i = 0; i < 30; i++)
            {
                ans += (" " + ss[i]);
            }

            return ans;
        }
        public static string Gremove_From_Seat( int seatnomber)
        {
            if (seats[seatnomber] == null)
            {
                return "ERR THE_SEAT_IS_USED";
            }
            try
            {
                int index = 999;
                for (int i = 0; i < groups.Count; i++)
                {
                    foreach (int j in groups[i].seatnombers)
                    {
                        if (j == seatnomber)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != 999)
                        break;
                }

                groups[index].Rerease(ref seats);

                groups.RemoveAt(index);
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

            return "OK GROUP_IS_REMOVED";
        }
        public static string Seat_Chec()
        {
            string ans = "OK";
            foreach (Group g in seats)
                ans += g == null ? " E" : " U";
            return ans;
        }
        public static string Get()
        {
            string ans = "OK " + groups.Count + " Groups Exists";
            foreach (Group g in groups)
                ans += "\r\n" + g.Print_Stat();
            return ans;
        }
    }
}












