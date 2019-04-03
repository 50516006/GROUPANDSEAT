using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class remove : MonoBehaviour {

    public int nomber;
    
    public void OnClick()
    {
        if (SceneManager.GetActiveScene().name == "remove")
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                bool f = false;
                DialogManager.Instance.SetLabel("YES", "NO", "CLOSE");
                DialogManager.Instance.ShowSelectDialog("Really?", "Nomber " + nomber + " is Selected.Do you truely want to remove?"
                  , (bool result) =>
                 {
                     if (result == true)
                     {
                         string msg = "CLOSE " + nomber;
                         TcpManager.temp = TcpManager.Transactor(msg);
                         SceneManager.LoadScene("main");
                     }
                 });
            }
            else
            {
                string msg = "CLOSE " + nomber;
                TcpManager.temp = TcpManager.Transactor(msg);
                SceneManager.LoadScene("main");

            }
        }
        else
        {
            TcpManager.chnomber = nomber;
            SceneManager.LoadScene("Chan2");


        }


    }
	// Use this for initialization
	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
        if (!Seatgetter.Seatflgs[nomber])
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
