using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class Numberplayers : MonoBehaviour
{
    public static int players;
    public static bool fun;

    public GameObject Name0;
    public GameObject Name1;
    public GameObject Name2;
    public GameObject Name3;
    public GameObject Name4;

    public TextMeshProUGUI PlayTimetext;

    public static string player0;
    public static string player1;
    public static string player2;
    public static string player3;
    public static string player4;

/*
    void Start(){
        PlayTimetext.text = PlayTime.ToString();

    }
*/
    void Start(){
        Debug.Log("aaaaaaaaaaaa");
        Debug.Log(player0);
        Debug.Log(player1);
        Debug.Log(player2);
        Debug.Log(player3);
        Debug.Log(player4);
        Name0.GetComponent<Text>().text = player0;

        Name1.GetComponent<Text>().text = player1;
        Name2.GetComponent<Text>().text = player2;
        Name3.GetComponent<Text>().text = player3;
        Name4.GetComponent<Text>().text = player4;

    }
   
}
