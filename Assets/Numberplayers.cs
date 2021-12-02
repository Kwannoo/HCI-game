using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Numberplayers : MonoBehaviour
{
    public static int players;
    public static bool fun;

    public GameObject Name0;
    public GameObject Name1;
    public GameObject Name2;
    public GameObject Name3;
    public GameObject Name4;

    public static string player0;
    public static string player1;
    public static string player2;
    public static string player3;
    public static string player4;

    void Update(){
        Name0.GetComponent<Text>().text = player0;
        Name1.GetComponent<Text>().text = player1;
        Name2.GetComponent<Text>().text = player2;
        Name3.GetComponent<Text>().text = player3;

    }
    
}
