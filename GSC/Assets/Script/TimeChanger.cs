using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeChanger : MonoBehaviour {
    public GameObject ip, port;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void OnClick () {
        int x = int.Parse(ip.GetComponent<Text>().text);
        int y = int.Parse(port.GetComponent<Text>().text);
        string z = "TC"+x+" "+y;
        TcpManager.temp=TcpManager.Transactor(z);
        SceneManager.LoadScene("main");

    }
}
