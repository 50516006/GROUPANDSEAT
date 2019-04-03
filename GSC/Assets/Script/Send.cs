using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Send : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void OnClick()
    {
        bool a=false;
        
        foreach(bool i in Setmanager.flg)
        {
            if (i)
            {
                a = true;
                break;
            }
        }
            

            string tag = Setmanager.Get_string();
        if (SceneManager.GetActiveScene().name == "Chan2")
                           {
            tag = Setmanager.Get_string2();
        }

            string s=tag.Substring(5);



                
            TcpManager.temp = TcpManager.Transactor(tag);
            

        Setmanager.Reset();
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");

    }
	// Update is called once per frame
	void Update () {
		
	}
}
