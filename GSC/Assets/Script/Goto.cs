using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goto : MonoBehaviour {
    public string target;
	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        SceneManager.LoadScene(target);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
