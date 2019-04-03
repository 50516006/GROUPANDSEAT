using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Flgsetter : MonoBehaviour {
    public int no;
	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        bool fg;
        fg=Setmanager.flg[no];
        Setmanager.flg[no] = !fg;


    }
	// Update is called once per frame
	void Update () {
        if (Seatgetter.Seatflgs[no])
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
