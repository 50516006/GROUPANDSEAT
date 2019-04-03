using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wpimg : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Change();
    }

	
    public void Change()
    {

        gameObject.SetActive( TcpManager.wpflg);
    }	
    
    // Update is called once per frame
	void Update () {
		
	}
}
