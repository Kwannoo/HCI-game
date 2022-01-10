using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
public class Gameoverr : MonoBehaviour
{
    public static float PlayTime;
    public static string Winner;

    public TextMeshProUGUI PlayTimetext;
    public TextMeshProUGUI Winnertext;

    // Start is called before the first frame update
    void Start()
    {
        PlayTimetext.text = "Total time of the game: " + PlayTime.ToString();
        Winnertext.text = "The winner is: " + Winner;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
