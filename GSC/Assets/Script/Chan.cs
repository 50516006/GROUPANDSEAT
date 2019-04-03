using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Chan : MonoBehaviour {
     public GameObject tag;
	// Use this for initialization
	void Start () {
		
	}
    public void OnClick()
    {
        TcpManager.temp =TcpManager.Transactor("CHS "+TcpManager.chnomber+" "+tag.GetComponent<Text>().text);
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");


    }	
	// Update is called once per frame
	void Update () {
		
	}
}
