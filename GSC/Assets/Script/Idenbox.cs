using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Idenbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = TcpManager.chnomber.ToString();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
