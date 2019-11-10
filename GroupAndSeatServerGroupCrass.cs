using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupAndSeat
{
    /// <summary>
    /// グループを表すクラス
    /// </summary>
    /// <param name="seatnombers">自クラスが使用する席の番号</param>
    /// <param name="start">開始時刻</param>
    class Group
    {
       public List<int> seatnombers;
        public DateTime start;
        
        /// <summary>
        /// コンストラクタ、指定された席番を確保し、入店時間を記録する
        /// </summary>
        /// <param name="seatnombers">使用する席の番号のリスト</param>
        /// <param name="seats">グループの参照を格納する席の集合</param>
        public  Group(List<int> seatnombers,ref Group[] seats)
        {

            this.seatnombers = seatnombers;
            //現在時刻を開始時刻として格納する
            start = DateTime.Now;
            //使用する席に自クラスの参照を格納する
            foreach(int i in seatnombers)
            {
                seats[i] = this;
            }

        }
       /// <summary>
       /// グループの情報を文字列で表示する
       /// </summary>
       /// <returns>グループの情報を表示するための文字列</returns>       
        public string Print_Stat()
        {
            //現在時刻と開始時刻の差分を取って経過時間を算出する
            var elasped= DateTime.Now-start;
            //経過時間と使用している席の番号を結合し、文字列を作成する
            string str = "Elasped Time " + elasped.ToString("hh':'mm")+" : Using Seat ";
            foreach (int i in seatnombers)
            {

                str += i + " ";

            }

            return (str);

        }
        /// <summary>
        /// 使用している席を開放する
        /// </summary>
        /// <param name="seats">席集合</param>
        public void Rerease(ref Group[] seats)
        {
            foreach(int i in seatnombers)
            {
                seats[i] = null;
            }
        }


    }
}
