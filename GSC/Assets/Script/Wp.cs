using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        if (TcpManager.wpflg==false) {
            TcpManager.wpflg = true;
        }
        else
        {
            TcpManager.wpflg = false;
        }


    }
	// Update is called once per frame
	void Update () {
		
	}
}
