using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loadgame : MonoBehaviour
{
    public GameObject Name0;
    public GameObject Name1;
    public GameObject Name2;
    public GameObject Name3;
    public GameObject Name4;


    public void Load(bool fun){
        Numberplayers.fun = fun;

        Numberplayers.player0 = Name0.GetComponent<Text>().text;
        Numberplayers.player1 = Name1.GetComponent<Text>().text;
        Numberplayers.player2 = Name2.GetComponent<Text>().text;
        Numberplayers.player3 = Name3.GetComponent<Text>().text;

        Debug.Log("voor de switch");
        switch (Numberplayers.players){
            case 2:
                SceneManager.LoadScene("2Players");
                break;
            case 3:
                SceneManager.LoadScene("3Players");
                break;
            case 4:
                SceneManager.LoadScene("4Players");
                break;
            case 5:
                SceneManager.LoadScene("5Players");
                break;
            default:
                Application.Quit();
                break;
        }
    }

}
