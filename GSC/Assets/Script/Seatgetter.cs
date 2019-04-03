using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Seatgetter : MonoBehaviour {
    static public bool[] Seatflgs;
    static public Color[] colors;
    


    int t;
	// Use this for initialization
	void Start () {
        Seatflgs = new bool[30];
        colors = new Color[30];
        
        string thissce= SceneManager.GetActiveScene().name;
        if (thissce == "svis")
        {
            SuperS();
        }
        else if (thissce == "Chan2") Seatsetter2();
        else{
            Seatsetter();
        }
        

	}
    private void Seatsetter2()
    {
        List<bool> anss = new List<bool>(); ;
        string ans = TcpManager.Transactor("SSC");
        string[] stat = ans.Split(' ');
        Debug.Log(ans);
        int nom = int.Parse(stat[TcpManager.chnomber+1]);
        Debug.Log(nom);
        for (int i = 1; i < 31; i++)
        {
            if ((int.Parse(stat[i]) == 999)|(int.Parse(stat[i])==nom))
            {
                anss.Add(false);

            }
            else
            {

                anss.Add(true);

            }
        }
        Seatflgs = anss.ToArray();




    }

	private void Seatsetter()
    {
        List<bool> anss = new List<bool>();
        string ans=TcpManager.Transactor("SC");
        string[] stat = ans.Split(' ');
        Debug.Log(ans);
        for(int i=1; i < 31; i++)
        {
            if ((stat[i] == "U")|(stat[i]=="U\n"))
            {
                anss.Add(true);
                Debug.Log((i-1)+" True");
            }
            else
            {
                Debug.Log((i-1)+" False");
                anss.Add(false);
            }
            

        }
        Seatflgs=anss.ToArray();

    }
    private void SuperS()
    {
        Color[] colorsample = {
            Color.black,
            Color.red,
            Color.grey,
            Color.yellow,
            Color.blue,
            Color.cyan,
            Color.magenta,
            Color.green,
            new Color(1,0.5f,1),
            new Color(0.5f,0,1),
            new Color(1,0.5f,1),
            new Color(0.5f,0,0.5f),
            new Color(1,0.45f,0),
            new Color(1,0.45f,0.8f),
            new Color(0,0.49f,0.65f),
            new Color(0,0.5f,0.18f),
            new Color(1,1,1,0.1f),
            new Color(1,1,1,0.2f),
            new Color(1,1,1,0.3f),
            new Color(1,1,1,0.4f),
            new Color(1,1,1,0.6f),
            new Color(1,1,1,0.7f),
            new Color(1,1,1,0.8f),
            new Color(1,1,1,0.9f),
            new Color(1,0.5f,1,0.5f),
            new Color(0.5f,0,1,0.5f),
            new Color(1,0.5f,1,0.5f),
            new Color(0.5f,0,0.5f,0.5f),
            new Color(1,0.45f,0,0.5f),
            new Color(1,0.45f,0.8f,0.5f) };
 

        List<Color> anss = new List<Color>();
        string ans=TcpManager.Transactor("SSC");
        string[] stat = ans.Split(' ');
        Debug.Log(ans);
 
        for(int i=1; i < 31; i++)
        {
            if (int.Parse(stat[i]) == 999)
            {
                anss.Add(Color.white);

            }
            else {

                anss.Add(colorsample[int.Parse(stat[i])]);

            }
        }
        colors = anss.ToArray();

    }
	// Update is called once per frame
	void Update () {

	}
}
