using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Minigame_manager2 : MonoBehaviour
{

    public int score;
    public static Minigame_manager2 inst;
    public Text scoreText;

    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score;
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
