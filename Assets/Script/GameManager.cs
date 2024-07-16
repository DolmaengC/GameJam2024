using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // public PlayerMovement playerMovement;
    public float gametime;
    public float maxGameTime = 5 * 5f;
    public int score;
    public TMP_Text scoreText;
    void Awake()
    {
        gametime = 0;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        score = 0;
        UpdateScore();
    }

    void FixedUpdate()
    {
        gametime += Time.deltaTime;

        if(gametime > maxGameTime){
            gametime = maxGameTime;
        }
    }

    void UpdateScore() {
        scoreText.text = "Score " + score.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScore();
    }
}
