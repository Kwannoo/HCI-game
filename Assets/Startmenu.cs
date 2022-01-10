using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Startmenu : MonoBehaviour
{


    public void ExitButton(){
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void StartGame(){
        SceneManager.LoadScene("Character");
    }

    public void LoadPlayers(int number){
        Numberplayers.players = number;

        switch (Numberplayers.players){
            case 2:
                SceneManager.LoadScene("2Names");
                break;
            case 3:
                SceneManager.LoadScene("3Names");
                break;
            case 4:
                SceneManager.LoadScene("4Names");
                break;
            case 5:
                SceneManager.LoadScene("5Names");
                break;
            default:
                Application.Quit();
                break;
        }
       // SceneManager.LoadScene("Gamemode");
    }

    // public void LoadNames(int number){
    //     switch(number){
    //         case 0:
    //             Numberplayers.player0 = InputText.GetComponent<Text>().text;
    //             break;
    //         case 1:
    //             Numberplayers.player1 = InputText.GetComponent<Text>().text;
    //             break;
    //         case 2:
    //             Numberplayers.player2 = InputText.GetComponent<Text>().text;
    //             break;
    //         case 3:
    //             Numberplayers.player3 = InputText.GetComponent<Text>().text;
    //             break;
    //         case 4:
    //             Numberplayers.player4 = InputText.GetComponent<Text>().text;
    //             break;
    //         default:
    //             break;

    //     }
    // }

    public void Load(bool fun){
        Numberplayers.fun = fun;
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

    public void StartBack(){
        SceneManager.LoadScene("Start");
    }
    public void CharacterBack(){
        SceneManager.LoadScene("Character");
    }
    public void Instructions1(){
        SceneManager.LoadScene("Instructions1");
    }
    public void Instructions2(){
        SceneManager.LoadScene("Instructions2");
    }
    public void Instructions3(){
        SceneManager.LoadScene("Instructions3");
    }

}
