using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class disp : MonoBehaviour {
    public GameObject txt;
	// Use this for initialization
	void Start () {
        txt.GetComponent<Text>().text = TcpManager.temp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
