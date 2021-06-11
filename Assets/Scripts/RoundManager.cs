using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    
    //PROPERTIES
    //time
    [SerializeField] private float startingTime;
    private float remainingTime;
    private bool canCoundDown = false;
    public TextMeshProUGUI timeText;
    
    //dogs count
    [SerializeField] private int startingDogsCount;
    private int currentDogsCount;
    public TextMeshProUGUI dogsCountText;

    //treats and snacks
    [SerializeField] private int currentTreatsCount;
    public TextMeshProUGUI treatsText;
    [SerializeField] private int currentSticksCount;
    public TextMeshProUGUI sticksCount;

    //pause
    private bool isPaused;

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
        remainingTime = startingTime;
        canCoundDown = true;
        _onRoundStart?.Invoke();
    }

    private void Update()
    {
        if (canCoundDown)
        {
            if (remainingTime <= 0)
            {
                canCoundDown = false;
                RoundEnd();
            }
            remainingTime -= Time.deltaTime;
            UpdateTimer();
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

    void RoundEnd()
    {
        _onRoundEnd?.Invoke();
        ShowScore();
    }

    void ShowScore()
    {
        int totalScore = 0;
        int dogScore = 0;
        int itemScore = 0;
        int timeScore = 0;

        dogScore = currentDogsCount * 20;
        itemScore = currentSticksCount * 10 + currentTreatsCount * 10;
        timeScore = (int)(100f * (remainingTime / startingTime));
        totalScore = timeScore + dogScore + itemScore;
    }
}
