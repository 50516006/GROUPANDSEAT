using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAndSeat
{
    class Group
    {
       public List<int> seatnombers;
        public DateTime start;

        public  Group(List<int> seatnombers,ref Group[] seats)
        {
            this.seatnombers = seatnombers;
            start = DateTime.Now;

            foreach(int i in seatnombers)
            {
                seats[i] = this;
            }

        }
        public string Print_Stat()
        {
            var elasped= DateTime.Now-start;
            string str = "Elasped Time " + elasped.ToString("hh':'mm")+" : Using Seat ";
            foreach (int i in seatnombers)
            {

                str += i + " ";

            }

            return (str);

        }

        public void Rerease(ref Group[] seats)
        {
            foreach(int i in seatnombers)
            {
                seats[i] = null;
            }
        }


    }
}
