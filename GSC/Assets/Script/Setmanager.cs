using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setmanager : MonoBehaviour {
    public static bool[] flg;
	// Use this for initialization
	void Start () {
        flg = new bool[30];

	}

    public static void Reset() {

        flg = new bool[30];

    }

    public static string Get_string()
    {
        string ans = "OPEN";

        for(int i =0;i<30;i++)
        {
            if (flg[i])
            {
                ans += " " + i;
            }
        }

        return ans;

    }


    public static string Get_string2()
    {
        string ans = "CHS "+TcpManager.chnomber+" ";
        for(int i =0;i<30;i++)
        {
            if (flg[i])
            {
                ans +=  i+",";
            }
        }

        return ans.Substring(0,ans.Length-1);






    }
	// Update is called once per frame
	void Update () {
		
	}
}
