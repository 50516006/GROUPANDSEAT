using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Save : MonoBehaviour {
    public GameObject ip, port;
	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        Debug.Log("Save");
        string portt = port.GetComponent<Text>().text;
        if (portt == "") {
            return;
        }

        string ipaddr= ip.GetComponent<Text>().text;
        if (ipaddr==""){
            return;

        }
        PlayerPrefs.SetString("IP",ipaddr);
        PlayerPrefs.SetString("PORT",portt);
        PlayerPrefs.Save();
    }
	void Update () {
		
	}
}
