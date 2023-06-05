using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public List<Transform> _players = new List<Transform>();
    public List<Transform> _deadPlayers = new List<Transform>();
    public int numPlayers = 0;
    public int numAlivePlayers = 0;

    public GameObject deathPanel;  
    public GameObject winnerPanel;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        else Instance = this;   
    }

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
                    ShowWinnerPanel();
                    break;
                }
            }
        }
    }

    public void PlayerDied(Transform player)
    {
        numAlivePlayers--;
        CheckForWinner();

        _deadPlayers.Add(player);

        foreach(var playerDead in _deadPlayers)
        {
            ShowDeathPanel();
        }
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

    private void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    private void ShowWinnerPanel()
    {
        winnerPanel.SetActive(true);
    }
}
