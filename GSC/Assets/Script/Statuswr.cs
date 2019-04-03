using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Statuswr : MonoBehaviour {
    public GameObject txxt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        string s="";
        for(int i= 0; i < 30; i++)
        {
            if (Setmanager.flg[i])
            {
                s += i + " ";
            }
        }
        txxt.GetComponent<Text>().text = s; 

	}
}
