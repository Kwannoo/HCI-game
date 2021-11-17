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
    public List<Transform> Player;

    public Transform Bomb;

    public GameObject Explosioneffect;

    public Text WordText;
    public Text WarningText;

    //List<GameObject> AmountPlayers;
    
    float speed = 5;
    float BombTime = 10f;

    private int CurrentBomb = 0;
    private int maxPlayers = 4;
    //private float speed = Time.deltaTime;
    List<string> wordlist = new List<string>();
    List<string> Wordssaid = new List<string>();

    protected DictationRecognizer dictationRecognizer;

    void Start() { //gets called by starting the game

        Readwordlist();
        WordText.text = "START!";
        WarningText.text = "";
        StartDictationEngine();
    }

    void Update(){ //get called every frame
        //Explodebomb();
        //Debug.Log(BombTime);
        MoveBomb();
    
    }

    private void Readwordlist(){
        var FileName = "wordlist.txt";
        var sr = new StreamReader(Application.dataPath + "/" + FileName);
        var fileContents = sr.ReadToEnd();
        sr.Close();
 
        var lines = fileContents.Split("\n"[0]);
        
        foreach (string line in lines){
            wordlist.Add(line);
        }
        
    }

    private void ChangeStatus(string text, ConfidenceLevel confidence){
       if (text == "status"){
            Debug.Log("Player 1 bomb: " + Player[0].GetComponent<Status>().hasBomb);
            Debug.Log("Player 2 bomb: " + Player[1].GetComponent<Status>().hasBomb);
            Debug.Log("Player 3 bomb: " + Player[2].GetComponent<Status>().hasBomb);
            Debug.Log("Player 4 bomb: " + Player[3].GetComponent<Status>().hasBomb);
        }
        if (text == "print"){
            foreach (string word in wordlist){
                Debug.Log(word);
            }
        }
        if (text == "explosion"){
            GameObject Explosion = Instantiate(Explosioneffect, Bomb.transform.position, Quaternion.identity);
            Destroy(Explosion, 1);
        }
        if (text == "ball"){
            NextPlayer();
        }
        if (text == "player"){
            Debug.Log(Player[0].GetComponent<Status>().hasBomb);
        }
    }
    
    private void Explodebomb(){
        BombTime -= Time.deltaTime;
        if (BombTime < 0){
            GameObject Explosion = Instantiate(Explosioneffect, Bomb.transform.position, Quaternion.identity);
            Destroy(Explosion, 1);
            Player[CurrentBomb].GetComponent<Status>().isDead = true;
            NextPlayer();
            BombTime = 10f;
        }
    }
    private void MoveBomb(){
        for (int i = 1; i <= maxPlayers; ++i){
            if (Player[CurrentBomb].GetComponent<Status>().hasBomb){
                float step = speed * Time.deltaTime;
                Vector2 playerpos = new Vector2(Player[CurrentBomb].transform.position.x+1.0f, Player[CurrentBomb].transform.position.y-0.5f);
                Bomb.transform.position = Vector2.MoveTowards(Bomb.transform.position, playerpos, step);
            }
        }
    }

    private void NextPlayer(){
        Debug.Log("Next player start");
        Debug.Log("CurrentBomb: "+CurrentBomb);

        for (int i = 0; i < Player.Count; ++i )
        {
            int playernumber = (i + CurrentBomb+1) % maxPlayers;
            Debug.Log("playernumber: " + playernumber);
            if (CurrentBomb+1 > maxPlayers){
                playernumber = 0;
            }
            Debug.Log("playernumber after: " + playernumber);
            if (!Player[playernumber].GetComponent<Status>().isDead){
             Debug.Log("check: " + playernumber);
                Player[CurrentBomb].GetComponent<Status>().hasBomb = false;

                    Player[playernumber].GetComponent<Status>().hasBomb = true;
                    Debug.Log("Bomb is passed");
                    CurrentBomb = playernumber;
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

    private void DictationRecognizer_OnDictationHypothesis(string text) //Fastest quality recognizer
    {
        Debug.Log("Dictation hypothesis: " + text);
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

