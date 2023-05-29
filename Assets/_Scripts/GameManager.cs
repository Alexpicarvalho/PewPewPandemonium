using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public List<Transform> _players = new List<Transform>();
    public int numPlayers = 0;
    public int numAlivePlayers = 0;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        else Instance = this;   
    }

    //void Start()
    //{
    //    if (Runner.IsServer)
    //    {
    //        // Start the shrinking animation on the master client
    //        //circleController.StartShrinking();
    //    }
    //}

    public override void Spawned()
    {       
        numPlayers = _players.Count;
        numAlivePlayers = numPlayers;
    }

    public void AddPlayerToList(Transform player)
    {
        if (!_players.Contains(player)) _players.Add(player);
    }

    void Update()
    {
        if (_players.Count == 0) return;

        // Check if the game is over
        if (IsGameOver())
        {
            if (Runner.IsServer)
            {
                CheckForWinner();
            }

            // End the game
            EndGame();
        }
    }

    void CheckForWinner()
    {
        if (numAlivePlayers == 1)
        {
            foreach (var player in _players)
            {
                if (player != null)
                {
                    Debug.Log(player.name + " wins!");
                    break;
                }
            }
        }
    }

    public void PlayerDied()
    {
        numAlivePlayers--;
        CheckForWinner();
    }

    private void EndGame()
    {
        Runner.SetActiveScene("Menu");
    }

    private bool IsGameOver()
    {
        if (numAlivePlayers <= 0)
            return true;
        else return false;
    }
}
