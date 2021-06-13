using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    
    //PROPERTIES
    //time
    [Header("Time properties")]
    [SerializeField] private float startingTime;
    private float remainingTime;
    private bool canCountDown = false;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeScoreText;
    
    [Header("Dog properties")]
    //dogs count
    [SerializeField] private int startingDogsCount;
    private int currentDogsCount;
    public TextMeshProUGUI dogsScoreText;

    [Header("Items properties")]
    //treats and snacks
    [SerializeField] private IntVariable currentTreatsCount;
    public TextMeshProUGUI treatsCountText;
    [Space(5)]
    [SerializeField] private IntVariable currentSticksCount;
    [Space(5)]
    public Image stickIcon;
    public Image stickIconInner;
    public Color colorUsable;
    public Color colorUnusable;
    [Space(5)]
    public TextMeshProUGUI itemsScoreText;

    //pause
    public bool isPaused = false;
    
    [Header("End screen/score properties")]
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
    public float raisingScoreRate = 0.08f;
    
    //canvases
    [Header("Canvases")]
    public GameObject gameCanvas;
    public GameObject pauseCanvas;
    
    [Header("Other properties")]
    [Space(20)]
    //other properties
    private bool isReplaying = false;

    public TextMeshProUGUI totalScoreText;
    public GameObject player;
    public Transform playerInitialPosition;
    public CinemachineVirtualCamera frontCamera;
    public List<Transform> possibleDogsPositions;
    public GameObject[] dogPrefabs;

    private List<GameObject> dogsList = new List<GameObject>();
    

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
            currentSticksCount.OnValueChange += OnStickCountChange;
            currentTreatsCount.OnValueChange += OnTreatCountChange;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    #region ROUND METHODS - ROUND START, SPAWN DOGS, UPDATE, ROUND END

    private void Start()
    {
        SpawnDogs();
    }

    public void StartRound()
    {
        print("Starting Round");

        player.transform.position = playerInitialPosition.transform.position;
        
        if (isReplaying)
        {
            foreach (var d in dogsList)
            {
                Destroy(d);
            }
            dogsList.Clear();
            SpawnDogs();
            isReplaying = false;
        }
        else
        {
            frontCamera.Priority = 9;
        }

        endScreen.SetActive(false);

        currentDogsCount = startingDogsCount;
        
        remainingTime = startingTime;
        canCountDown = true;
        
        gameCanvas.SetActive(true);
        _onRoundStart?.Invoke();
    }

    private void SpawnDogs()
    {
        var usableDogsPositions = possibleDogsPositions.OrderBy(x => Random.value).Take(5);
        foreach (var pos in usableDogsPositions)
        {
            print("SPAWNING DOGGO");
            var newDog = Instantiate(dogPrefabs[Random.Range(0,dogPrefabs.Length)], pos.transform.position, Quaternion.identity, null);
            newDog.GetComponent<SpringJoint>().connectedBody = player.GetComponentInChildren<Rigidbody>();
            dogsList.Add(newDog);
        }
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
    
    public void RoundEndTest()
    {
        print("Calling RoundEnd! (this is a test)");
        RoundEnd();
    }

    void RoundEnd()
    {
        print("Round Ended!");
        canCountDown = false;
        _onRoundEnd?.Invoke();
        ShowScore();
    }
    #endregion

    #region GAME METHODS - STICKS AND TREATS

    private void OnTreatCountChange()
    {
        UpdateTreatsText();
    }

    private void OnStickCountChange()
    {
        if (currentSticksCount.Value > 0)
        {
            stickIcon.color = colorUsable;
            stickIconInner.color = colorUsable;
        }
        else
        {
            stickIcon.color = colorUnusable;
            stickIconInner.color = colorUnusable;
        }
    }

    public void UpdateTreatsText()
    {
        treatsCountText.text = currentTreatsCount.Value.ToString();
    }
    #endregion

    #region PAUSE METHODS
    public void TogglePause()
    {
        print("Toggled pause: " + !isPaused);
        if (!isPaused)
        {
            pauseCanvas.SetActive(true);
            _onPause?.Invoke();
            Time.timeScale = 0;
        }
        else
        {
            pauseCanvas.SetActive(false);
            _onResume?.Invoke();
            Time.timeScale = 1;
        }

        isPaused = !isPaused;
    }
    #endregion

    #region SCORE METHODS - SHOW AND UPDATE SCORES
    void ShowScore()
    {
        print("Showing Score");
        endScreen.SetActive(true);
        
        //resetting final scores before calculations
        playerTotalScore = playerDogsScore =  playerItemScore = playerTimeScore = 0;
        playerRaisingTotalScore = playerRaisingDogsScore = playerRaisingItemScore = playerRaisingTimeScore = 0f;
        
        //score calculations
        playerDogsScore = currentDogsCount * 20;
        playerItemScore = currentSticksCount.Value * 10 + currentTreatsCount.Value * 10;
        playerTimeScore = (int)(100f * (remainingTime / startingTime));
        playerTotalScore = playerTimeScore + playerDogsScore + playerItemScore;

        //setting base score values for the texts
        dogsScoreText.text = "0";
        itemsScoreText.text = "0";
        timeScoreText.text = "0";
        totalScoreText.text = "0";
        
        print("Player scores - Dogs: " + playerDogsScore + " - Items: " + playerItemScore + " - Time: " + playerTimeScore + " - Total: " + playerTotalScore);

        isShowingScores = true;
    }

    void RaiseDogsScore()
    {
        if (playerRaisingDogsScore < playerDogsScore)
        {
            playerRaisingDogsScore += raisingScoreRate;
            dogsScoreText.text = ((int)playerRaisingDogsScore).ToString();
        }
    }

    void RaiseItemScore()
    {
        if (playerRaisingItemScore < playerItemScore)
        {
            playerRaisingItemScore += raisingScoreRate;
            itemsScoreText.text = ((int)playerRaisingItemScore).ToString();
        }
    }

    void RaiseTimeScore()
    {
        if (playerRaisingTimeScore < playerTimeScore)
        {
            playerRaisingTimeScore += raisingScoreRate;
            timeScoreText.text = ((int)playerRaisingTimeScore).ToString();
        }
    }

    void RaiseTotalScore()
    {
        if (playerRaisingTotalScore < playerTotalScore)
        {
            playerRaisingTotalScore += raisingScoreRate;
            totalScoreText.text = ((int)playerRaisingTotalScore).ToString();
        }
    }
    #endregion
    
    #region SCENE METHODS - RESTART, MENU AND QUIT
    public void RestartGame()
    {
        print("Restarting Game");
        isReplaying = true;
        StartRound();
    }

    public void BackToMenu()
    {
        print("Going back to menu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        print("Quitting");
        Application.Quit();
    }
    #endregion
}
