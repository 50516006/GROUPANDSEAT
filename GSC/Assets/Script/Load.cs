using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Load : MonoBehaviour {

    public GameObject ip, port;
	// Use this for initialization
	void Start () {
		
	}
	
	public void OnClick()
    {
        string ipt = PlayerPrefs.GetString("IP");
        string  portt= PlayerPrefs.GetString("PORT");
        TcpManager.ipaddr=ipt;
        TcpManager.port=int.Parse(portt);
        TcpManager.temp = "";
        SceneManager.LoadScene("main");

    }
	// Update is called once per frame
	void Update () {
		
	}
}
