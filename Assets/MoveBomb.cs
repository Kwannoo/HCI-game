// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MoveBomb : MonoBehaviour
// {
//     public Transform Player1;
//     public Transform Player2;
//     public Transform Player3;
//     public Transform Player4;

//     public Transform Bomb;

//     float speed;
//     int maxPlayers = 4;


//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         for (int i = 0; i < maxPlayers; ++i){
//             GameObject PlayerBomb = GameObject.Find("Player" + i);
//             if (PlayerBomb.GetComponent<Status>().hasBomb){
//                 Debug.Log(PlayerBomb);
//                 float step = speed * Time.deltaTime;
//                 Bomb.transform.position = Vector2.MoveTowards(Bomb.transform.position, PlayerBomb.transform.position, step);
//             }
//         }
//     }
// }
