using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // public PlayerMovement playerMovement;
    public float gametime;
    public float maxGameTime = 5 * 5f;
    public TMP_Text timerText;
    public Slider timerBar;
    public GameObject gameClearPanel;
    public GameObject gameOverPanel;
    public Button gameClearOKButton;
    public Button gameOverOKButton;
    public int score;
    public TMP_Text scoreText;
    bool isPaused;

    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1;
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
        gameClearOKButton.onClick.AddListener(OnGoHomeButtonClicked);
        gameOverOKButton.onClick.AddListener(OnGoHomeButtonClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TogglePause();
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            gametime -= Time.deltaTime;
            UpdateTimer();

            if (gametime <= 0)
            {
                EndGame(true);
            }
        }
    }

    void UpdateScore()
    {
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

    public void EndGame(bool isClear)
    {
        gametime = 0;
        isPaused = true;
        Time.timeScale = 0;
        if (isClear) {
            gameClearPanel.SetActive(true);
        }
        else {
            gameOverPanel.SetActive(true);
        }
    }

    void OnGoHomeButtonClicked()
    {
        SceneManager.LoadScene("HomeScene"); 
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
