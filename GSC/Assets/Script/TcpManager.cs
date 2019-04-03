using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;

public class TcpManager : MonoBehaviour {
    public static string ipaddr;
    public static int port;
    public static string temp;
    public static int chnomber;
    public static  bool wpflg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private static string Send_Resv(string str, System.Net.Sockets.NetworkStream strm)
    {
        var enc = System.Text.Encoding.UTF8;
        byte[] sendBytes = enc.GetBytes(str + '\n');
        strm.Write(sendBytes, 0, sendBytes.Length);
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        byte[] resBytes = new byte[256];
        int resSize = 0;
        do
        {
            resSize = strm.Read(resBytes, 0, resBytes.Length);
            if (resSize == 0)
            {
                break;
            }
            ms.Write(resBytes, 0, resSize);
        } while (strm.DataAvailable || resBytes[resSize - 1] != '\n');
        string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        ms.Close();
        strm.Close();
        return resMsg;
    }
    private static string Wssend(string msg)
    {
        string ans="";
        System.Net.WebRequest request = System.Net.HttpWebRequest.Create("http://"+ipaddr+":"+port);
        request.Method = "GET";
        System.Net.WebResponse resp = null;
        try
        {
            resp = request.GetResponse();
        }
        catch
        {
            resp = null;
        }

        if (resp != null)
        {
            
            Stream st = resp.GetResponseStream();
            StreamReader sr = new StreamReader(st, Encoding.GetEncoding("UTF-8"));
            ans = sr.ReadToEnd();
            sr.Close();
            st.Close();
        }
        return ans; 
    }
    public static string Transactor(string msg)

    {
        string ans;

        try
        {
            var soc = new System.Net.Sockets.TcpClient(ipaddr, port);

            soc.SendTimeout = 10000;
            soc.ReceiveTimeout = 10000;
            var strm = soc.GetStream();
            ans = Send_Resv(msg, strm);
        }
        catch(Exception ex)
        {
            ans = ex.Message+"\n";
            ans += ex.StackTrace;
        }
        return ans;
    }



}
