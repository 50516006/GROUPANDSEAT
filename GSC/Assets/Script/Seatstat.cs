using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Seatstat : MonoBehaviour {
    public int nom;
	// Use this for initialization
	void Start () {
        nom = int.Parse(this.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "vis")
        {
            bool fl = Seatgetter.Seatflgs[nom];
            if (fl)
            {
                this.gameObject.GetComponent<Image>().color = Color.red;
            }
            else
            {

                this.gameObject.GetComponent<Image>().color = Color.blue;
            }
        }
        else
        {

                this.gameObject.GetComponent<Image>().color = Seatgetter.colors[nom];




        }
	
	}
}
