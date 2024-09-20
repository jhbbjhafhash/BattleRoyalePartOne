using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
   [Header("Players")]
   public string playerPrefabLocation;
   public PlayerController[] players;
   public Transform[] spawnPoints;
   public int alivePlayers;

   private int playersInGame;

   public static GameManager instance;

   void Awake()
   {
        instance = this;
   }

   void Start()
   {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        alivePlayers = players.Length;

        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
   }
    
    [PunRPC]
   void ImInGame()
   {
        playersInGame++;

        if(PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
            photonView.RPC("SpawnPlayer", RpcTarget.All);

   }

    [PunRPC]
   void SpawnPlayer()
   {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        playerObj.GetComponent<PlayerController>().photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
   }
}
