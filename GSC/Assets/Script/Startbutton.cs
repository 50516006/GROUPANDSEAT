using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Startbutton : MonoBehaviour {
    public GameObject ip, port;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void OnClick()
    {


        TcpManager.ipaddr= ip.GetComponent<Text>().text;


        TcpManager.port = int.Parse(port.GetComponent<Text>().text);
        TcpManager.temp = "";
        SceneManager.LoadScene("main");


    }

}
