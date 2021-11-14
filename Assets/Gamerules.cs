using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using System.IO;

public class Gamerules : MonoBehaviour
{
    public Transform Player1;
    public Transform Player2;
    public Transform Player3;
    public Transform Player4;

    public Transform Bomb;

    public Text WordText;
    public Text WarningText;

    List<GameObject> AmountPlayers;
    

    private int CurrentBomb = 1;
    private int maxPlayers = 4;
    //private float speed = Time.deltaTime;
    List<string> wordlist = new List<string>();
    List<string> Wordssaid = new List<string>();



    protected DictationRecognizer dictationRecognizer;
    void Start() //gets called by starting the game
    {
        Readwordlist();
        WordText.text = "START!";
        WarningText.text = "";
        StartDictationEngine();
    }

    void Update(){ //get called every frame
       // MoveBomb();
    }

    private void Readwordlist(){
        var FileName = "wordlist.txt";
        var sr = new StreamReader(Application.dataPath + "/" + FileName);
        var fileContents = sr.ReadToEnd();
        sr.Close();
 
        var lines = fileContents.Split("\n"[0]);
        
        foreach (string line in lines){
            wordlist.Add(line);
            Debug.Log(line);
        }
        
    }

    private void DictationRecognizer_OnDictationHypothesis(string text) //Fastest quality recognizer
    {
        Debug.Log("Dictation hypothesis: " + text);
    }

    private void ChangeStatus(string text, ConfidenceLevel confidence){
        //WordText.text = text;
        //CheckWord(text);
        if (CheckWord(text)){
            NextPlayer();
            Debug.Log("doorgeven");
        }
        if (text == "status"){
            Debug.Log("Player 1 bomb: " + Player1.GetComponent<Status>().hasBomb);
            Debug.Log("Player 2 bomb: " + Player2.GetComponent<Status>().hasBomb);
            Debug.Log("Player 3 bomb: " + Player3.GetComponent<Status>().hasBomb);
            Debug.Log("Player 4 bomb: " + Player4.GetComponent<Status>().hasBomb);
        }
        if (text == "print"){
            foreach (string word in wordlist){
                Debug.Log(word);
            }
        }
    }
    
    // private void MoveBomb(){
    //     for (int i = 0; i < maxPlayers; ++i){
    //         GameObject PlayerBomb = GameObject.Find("Player" + i);
    //         // if (PlayerBomb.GetComponent<Status>().hasBomb){
    //         //     Debug.Log(PlayerBomb);
    //         //     float step = speed * Time.deltaTime;
    //         //     Bomb.transform.position = Vector2.MoveTowards(Bomb.transform.position, PlayerBomb.transform.position, step);
    //         // }
    //     }
    // }

    private void NextPlayer(){
        Debug.Log("Next player start");
        Debug.Log("CurrentBomb: "+CurrentBomb);

        for (int i = 0; i < maxPlayers; ++i )
        {
            GameObject nextPlayer = GameObject.Find("Player" + (i + CurrentBomb+1) % (maxPlayers + 1));
            Debug.Log("next player: " + nextPlayer);
            if (!nextPlayer.GetComponent<Status>().isDead){
                GameObject currentPlayer = GameObject.Find("Player" + CurrentBomb);
                
                currentPlayer.GetComponent<Status>().hasBomb = false;

                    nextPlayer.GetComponent<Status>().hasBomb = true;
                    Debug.Log("Bomb is passed");
                    CurrentBomb++;
                    break;
            }
            
        }
    }

    private bool CheckWord(string text){
        foreach(string item in wordlist) {
            if(item.Contains(text)){
                foreach(string item1 in Wordssaid){
                    if(item1.Contains(text)){
                        WordText.text = text;
                        WordText.GetComponent<Text>().color = Color.red;
                        WarningText.text = "This word has already been said";
                        return false;
                    }
                }
                WordText.text = text;
                WordText.GetComponent<Text>().color = Color.green;
                WarningText.text = "";
                Wordssaid.Add(text);
                return true;
            }

        }
        WordText.text = text;
        WordText.GetComponent<Text>().color = Color.red;
        WarningText.text = "This word is not in the wordlist!";
        return false;
    }
    
    private void DictationRecognizer_OnDictationComplete(DictationCompletionCause completionCause) //Is used for recognizing the word
    {
        switch (completionCause)
        {
            case DictationCompletionCause.TimeoutExceeded:
            case DictationCompletionCause.PauseLimitExceeded:
            case DictationCompletionCause.Canceled:
            case DictationCompletionCause.Complete:
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.UnknownError:
            case DictationCompletionCause.AudioQualityFailure:
            case DictationCompletionCause.MicrophoneUnavailable:
            case DictationCompletionCause.NetworkFailure:
                // Error
                CloseDictationEngine();
                break;
        }
    }
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence) //Highest quality recognizer
    {
        Debug.Log("Dictation result: " + text);
    }
    private void DictationRecognizer_OnDictationError(string error, int hresult)
    {
        Debug.Log("Dictation error: " + error);
    }
    private void OnApplicationQuit()
    {
        CloseDictationEngine();
    }
    private void StartDictationEngine() //Handles the spoken word
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.DictationResult += ChangeStatus;
        dictationRecognizer.Start();
    }
    private void CloseDictationEngine() //Resets the Dictionary
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis;
            dictationRecognizer.DictationComplete -= DictationRecognizer_OnDictationComplete;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            dictationRecognizer.DictationError -= DictationRecognizer_OnDictationError;
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
    }

        
}

