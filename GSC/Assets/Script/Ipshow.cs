using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ipshow : MonoBehaviour {
    public GameObject a;

	// Use this for initialization
	void Start () {
        a.GetComponent<Text>().text = TcpManager.ipaddr + ":" + TcpManager.port;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
