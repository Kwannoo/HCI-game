using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class Gamerules : MonoBehaviour
{
    public List<Transform> Player;

    public Transform Bomb;

    public GameObject Explosioneffect;

    public TextMeshProUGUI WordText;
    //public Text WordText;
    public Text WarningText;

    public AudioSource Correctsound;
    public AudioSource Incorrectsound;
    public AudioSource Backgroundsound;
    //List<GameObject> AmountPlayers;
    
    public Image TimeBar;

    float speed = 5;

    public float TotalTime = 30f;
    public float BombTime = 30f;

    private int CurrentBomb = 0;
    private int maxPlayers = 0;
    private int PlayersAlive;
    //private float speed = Time.deltaTime;
    List<string> wordlist = new List<string>();
    List<string> Wordssaid = new List<string>();

    protected DictationRecognizer dictationRecognizer;

    void Start() { //gets called by starting the game
        maxPlayers = Player.Count;
        Gameoverr.PlayTime = 0;
        PlayersAlive = maxPlayers;
        Readwordlist();
        WordText.text = "START!";
        WarningText.text = "";
        StartDictationEngine();
        //Backgroundsound.pitch = 1.5f;
    }

    void Update(){ //get called every frame
        Explodebomb();
        //Debug.Log(BombTime);
        Changepitch();
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

    private void Changepitch(){
        float speed = 1f + (0.07f*(TotalTime-BombTime)); //change constant
        Backgroundsound.pitch = speed;
        //Debug.Log(speed);
    }

    private void ChangeStatus(string text){//, ConfidenceLevel confidence){
        if (CheckWord(text)){
            NextPlayer();
        }
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

    private string FindName(int winner){
        switch(winner){
            case 0:
                return Numberplayers.player0;
                break;
            case 1:
                return Numberplayers.player1;
                break;
            case 2:
                return Numberplayers.player2;
                break;
            case 3:
                return Numberplayers.player3;
                break;
            case 4:
                return Numberplayers.player4;
                break;

        } 
        return "";
    }
    
    private void Explodebomb(){
        BombTime -= Time.deltaTime;
        Gameoverr.PlayTime += Time.deltaTime;
        TimeBar.fillAmount = BombTime/TotalTime;
        if (BombTime < 0){
            PlayersAlive--;
            GameObject Explosion = Instantiate(Explosioneffect, Bomb.transform.position, Quaternion.identity);
            Destroy(Explosion, 1);
            Player[CurrentBomb].GetComponent<Status>().isDead = true;
            string playerout = FindName(CurrentBomb);
            WordText.GetComponent<TextMeshProUGUI>().color = Color.red;
            WordText.text = playerout + " is out of the game!";
            WarningText.text = "";

            NextPlayer();
            if (PlayersAlive == 1){
                Gameoverr.Winner = FindName(CurrentBomb);
                //Gameoverr.Winner = CurrentBomb;
                SceneManager.LoadScene("Gameover");
            }
            BombTime = TotalTime;
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
                        WordText.GetComponent<TextMeshProUGUI>().color = Color.red;
                        WarningText.GetComponent<Text>().color = Color.red;
                        WarningText.text = "This word has already been said";
                        Incorrectsound.Play();
                        return false;
                    }
                }
                WordText.text = text;
                WordText.GetComponent<TextMeshProUGUI>().color = Color.green;
                WarningText.GetComponent<Text>().color = Color.green;
                WarningText.text = "Good job!";
                Wordssaid.Add(text);
                Correctsound.Play();
                BombTime += 2f;
                return true;
            }

        }
        WordText.text = text;
        WordText.GetComponent<TextMeshProUGUI>().color = Color.red;
        WarningText.GetComponent<Text>().color = Color.red;
        WarningText.text = "This word is not in the wordlist!";
        Incorrectsound.Play();
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
        dictationRecognizer.DictationHypothesis += ChangeStatus;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
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

