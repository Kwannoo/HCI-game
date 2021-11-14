using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class Gamerulesrandom : MonoBehaviour
{
    public Transform Player1;
    public Transform Player2;
    public Transform Player3;
    public Transform Player4;

    List<GameObject> AmountPlayers;

    private int CurrentBomb = 1;
    private int maxPlayers = 4;

    protected DictationRecognizer dictationRecognizer;
    void Start() //gets called by starting the game
    {
        
        StartDictationEngine();
    }

    void Update(){ //get called every frame

    
    }

    private void DictationRecognizer_OnDictationHypothesis(string text) //Fastest quality recognizer
    {
        Debug.Log("Dictation hypothesis: " + text);
    }

    private void ChangeStatus(string text, ConfidenceLevel confidence){
        if (text == "pass"){
            NextPlayer();
            Debug.Log("doorgeven");
        }
        if (text == "status"){
            Debug.Log("Player 1 bomb: " + Player1.GetComponent<Status>().hasBomb);
            Debug.Log("Player 2 bomb: " + Player2.GetComponent<Status>().hasBomb);
            Debug.Log("Player 3 bomb: " + Player3.GetComponent<Status>().hasBomb);
            Debug.Log("Player 4 bomb: " + Player4.GetComponent<Status>().hasBomb);
        }
    }

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
                    break;
            }
            
        }
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

