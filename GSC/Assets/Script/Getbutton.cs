using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Getbutton : MonoBehaviour {
    public GameObject text;
	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        string ans=TcpManager.Transactor("GET");
        text.GetComponent<Text>().text = ans;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
