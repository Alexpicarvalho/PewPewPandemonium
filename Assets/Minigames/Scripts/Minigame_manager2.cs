using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Minigame_manager2 : MonoBehaviour
{

    public int score;
    public static Minigame_manager2 inst;
    [SerializeField] Text scoreText;
    [SerializeField] MG_Playermovement playermovement;

    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score;

        //increase playyer speed
        playermovement.speed += playermovement.speedIncreasePerPoint;
    }

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
