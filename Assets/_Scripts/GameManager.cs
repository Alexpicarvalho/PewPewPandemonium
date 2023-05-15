using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class GameManager : NetworkBehaviour
{
    //public CircleHandler circle;
    public List<NetworkPlayer> players = new List<NetworkPlayer>();
    //[SyncVar]
    public int numPlayers;
    //[SyncVar]
    public int numAlivePlayers;

    void Start()
    {
        if (Runner.IsServer)
        {
            // Start the shrinking animation on the master client
            //circleController.StartShrinking();
        }
    }

    public override void Spawned()
    {
        numPlayers = players.Count;
        numAlivePlayers = numPlayers;
    }

    void Update()
    {
        // Check if the game is over
        if (IsGameOver())
        {
            if (Runner.IsServer)
            {
                // Stop the shrinking animation on the master client
                //circleController.StopShrinking();
                //CheckForWinner();
            }

            // End the game
            EndGame();
        }
    }

    //[Networked]
    //void CheckForWinner()
    //{
    //    if (numAlivePlayers == 1)
    //    {
    //        foreach (NetworkPlayer player in players)
    //        {
    //            if (player.isAlive)
    //            {
    //                Debug.Log(player.name + " wins!");
    //                break;
    //            }
    //        }
    //    }
    //}

    //[Networked]
    //public void PlayerDied()
    //{
    //    numAlivePlayers--;
    //    CheckForWinner();
    //}

    private void EndGame()
    {
        //throw new NotImplementedException();
    }

    private bool IsGameOver()
    {
        if (numAlivePlayers <= 0)
            return true;
        else return false;
    }
}
