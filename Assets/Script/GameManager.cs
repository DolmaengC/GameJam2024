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
    public TMP_Text timerText;
    public Slider timerBar;
    public int score;
    public TMP_Text scoreText;
    bool isPaused = false;
    void Awake()
    {
        gametime = maxGameTime;
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                isPaused = true;
                return;
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                return;
            }
        }
    }
    void FixedUpdate()
    {
        gametime -= Time.deltaTime;
        UpdateTimer();

        if(gametime <= 0){
            
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

    void UpdateTimer()
    {
        int min = Mathf.FloorToInt(gametime / 60);
        int sec = Mathf.FloorToInt(gametime % 60);
        timerText.text = min.ToString() + ":" + sec.ToString().PadLeft(2, '0');
        timerBar.maxValue = maxGameTime;
        timerBar.value = maxGameTime - gametime;
        
    }
}
