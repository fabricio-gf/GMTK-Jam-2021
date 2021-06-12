using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    
    //PROPERTIES
    //time
    [SerializeField] private float startingTime;
    private float remainingTime;
    private bool canCountDown = false;
    public TextMeshProUGUI timeText;
    
    public TextMeshProUGUI timeCountText;
    
    
    //dogs count
    [SerializeField] private int startingDogsCount;
    private int currentDogsCount;
    public TextMeshProUGUI dogsCountText;
    

    //treats and snacks
    [SerializeField] private int currentTreatsCount;
    public TextMeshProUGUI treatsCountText;
    [SerializeField] private int currentSticksCount;
    public TextMeshProUGUI sticksCountText;

    public TextMeshProUGUI itemsCountText;

    public TextMeshProUGUI totalCountText;

    //pause
    private bool isPaused;
    
    //end screen/scores
    public GameObject endScreen;
    private bool isShowingScores;
    private int playerTotalScore;
    private float playerRaisingTotalScore;
    private int playerTimeScore;
    private float playerRaisingTimeScore;
    private int playerDogsScore;
    private float playerRaisingDogsScore;
    private int playerItemScore;
    private float playerRaisingItemScore;
    private float raisingScoreRate = 0.08f;

    //DELEGATES
    public delegate void OnRoundStart();
    public OnRoundStart _onRoundStart;

    public delegate void OnRoundEnd();
    public OnRoundEnd _onRoundEnd;
    
    public delegate void OnPause();
    public OnPause _onPause;

    public delegate void OnResume();
    public OnResume _onResume;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        currentDogsCount = startingDogsCount;
        currentSticksCount = 2; //temp
        currentTreatsCount = 3; //temp
        
        remainingTime = startingTime;
        canCountDown = true;
        _onRoundStart?.Invoke();
    }

    private void Update()
    {
        if (canCountDown)
        {
            if (remainingTime <= 0)
            {
                RoundEnd();
            }
            remainingTime -= Time.deltaTime;
            UpdateTimer();
        }

        if (isShowingScores)
        {
            RaiseDogsScore();

            RaiseItemScore();            
            
            RaiseTimeScore();

            RaiseTotalScore();
        }
    }

    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void PickupStick()
    {
        currentSticksCount++;
    }

    void TogglePause()
    {
        if (isPaused)
        {
            _onPause?.Invoke();
            
        }
        else
        {
            _onResume?.Invoke();
            
        }

        isPaused = !isPaused;
    }

    public void RoundEndTest()
    {
        RoundEnd();
    }

    void RoundEnd()
    {
        canCountDown = false;
        _onRoundEnd?.Invoke();
        ShowScore();
    }

    void ShowScore()
    {
        endScreen.SetActive(true);
        
        playerTotalScore = 0;
        playerDogsScore = 0;
        playerItemScore = 0;
        playerTimeScore = 0;

        playerDogsScore = currentDogsCount * 20;
        playerItemScore = currentSticksCount * 10 + currentTreatsCount * 10;
        playerTimeScore = (int)(100f * (remainingTime / startingTime));
        playerTotalScore = playerTimeScore + playerDogsScore + playerItemScore;

        isShowingScores = true;
    }
    
    void RaiseDogsScore()
    {
        if (playerRaisingDogsScore < playerDogsScore)
        {
            playerRaisingDogsScore += raisingScoreRate;
            dogsCountText.text = ((int)playerRaisingDogsScore).ToString();
        }
    }

    void RaiseItemScore()
    {
        if (playerRaisingItemScore < playerItemScore)
        {
            playerRaisingItemScore += raisingScoreRate;
            itemsCountText.text = ((int)playerRaisingItemScore).ToString();
        }
    }

    void RaiseTimeScore()
    {
        if (playerRaisingTimeScore < playerTimeScore)
        {
            playerRaisingTimeScore += raisingScoreRate;
            timeCountText.text = ((int)playerRaisingTimeScore).ToString();
        }
    }

    void RaiseTotalScore()
    {
        if (playerRaisingTotalScore < playerTotalScore)
        {
            playerRaisingTotalScore += raisingScoreRate;
            totalCountText.text = ((int)playerRaisingTotalScore).ToString();
        }
    }
}
