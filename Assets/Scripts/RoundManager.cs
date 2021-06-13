using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    
    public enum Dificulty
    {
        Mixed = 0,
        Small,
        Medium,
        Big,
    }

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
    private int currentDogsCount
    {
        get => dogsList.Count;
    }
    public TextMeshProUGUI dogsScoreText;

    [Header("Dificulty options")]
    protected Dificulty currentDificulty = Dificulty.Mixed;
    public TMP_Dropdown dificultyDropdown;
    public Slider dogCountSlider;

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
    private bool isShowingScores = false;
    private bool isShowingTimeScore = false;
    private bool isShowingDogsScore = false;
    private bool isShowingItemsScore = false;
    private bool isShowingTotalScore = false;

    private int playerTotalScore;
    private float playerRaisingTotalScore;
    private int playerTimeScore;
    private float playerRaisingTimeScore;
    private int playerDogsScore;
    private float playerRaisingDogsScore;
    private int playerItemScore;
    private float playerRaisingItemScore;
    public float raisingScoreRate = 0.08f;

    public TextMeshProUGUI punResultText;
    
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
    public CinemachineVirtualCamera endCamera;
    public List<Transform> possibleDogsPositions;
    public GameObject[] dogPrefabs;

    [HideInInspector]
    public List<GameObject> dogsList = new List<GameObject>();

    public Animator playerAnimator;
    

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
            //currentSticksCount.OnValueChange += OnStickCountChange;
            //currentTreatsCount.OnValueChange += OnTreatCountChange;
        }
        
        //DontDestroyOnLoad(gameObject);
        GetFromSave();
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
            SpawnDogs(true);
            isReplaying = false;
        }
        else
        {
            frontCamera.Priority = 9;
        }

        endScreen.SetActive(false);

        remainingTime = startingTime;
        canCountDown = true;
        
        gameCanvas.SetActive(true);
        _onRoundStart?.Invoke();
        SaveSettings();
    }

    private void ClearDogs()
    {
        foreach (var d in dogsList)
        {
            Destroy(d);
        }
        dogsList.Clear();
    }

    private void RemoveDogAmount(int dogsToKeep)
    {
        int toRemove = (dogsToKeep >= 0)? dogsToKeep : 0;
        while(toRemove > 0 && dogsList.Count > 0)
        {
            Destroy(dogsList[dogsList.Count - 1]);
            dogsList.RemoveAt(dogsList.Count-1);
        }
    }

    private void SpawnDogs(bool clearAll = true)
    {
        if(clearAll)
            ClearDogs();
        else
            RemoveDogAmount(currentDogsCount - startingDogsCount);
        
        int toSpawn = startingDogsCount - currentDogsCount;
        for (int i = 0; i < toSpawn; i++)
        {
            SpawnDog();
        }
    }

    private void SpawnDog()
    {
        var usableDogsPositions = possibleDogsPositions.OrderBy(x => Random.value).Take(1);
        foreach (var pos in usableDogsPositions)
        {
            print("SPAWNING DOGGO");
            int spawnId = Random.Range(0,dogPrefabs.Length);
            switch(currentDificulty)
            {
                case Dificulty.Small:
                    spawnId = 0;
                    break;
                case Dificulty.Medium:
                    spawnId = 1;
                    break;
                case Dificulty.Big:
                    spawnId = 2;
                    break;
            }
            var newDog = Instantiate(dogPrefabs[spawnId], pos.transform.position, Quaternion.identity, null);
            newDog.GetComponent<SpringJoint>().connectedBody = player.GetComponentInChildren<Rigidbody>();
            dogsList.Add(newDog);
        }
    }

    private void Update()
    {
        if (canCountDown)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                RoundEndDefeat();
            }
            else
            {
                UpdateTimer();
            }
        }

        if (isShowingScores)
        {
            if (isShowingTimeScore)
            {
                RaiseTimeScore();
            }
            
            if (isShowingDogsScore)
            {
                RaiseDogsScore();
            }

            if (isShowingItemsScore)
            {
                RaiseItemScore();
            }

            if (isShowingTotalScore)
            {
                RaiseTotalScore();
            }
        }
    }

    private void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    void RoundEndDefeat()
    {
        print("Round Ended :(");
        canCountDown = false;
        
        //hard coding stuff
        gameCanvas.transform.Find("GameHUD").gameObject.SetActive(false);
        foreach (var d in dogsList)
        {
            d.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        _onRoundEnd?.Invoke();
        ShowScore();
    }

    public void RoundEndVictory()
    {
        print("Round Ended!");
        canCountDown = false;
        
        //hard coding stuff
        gameCanvas.transform.Find("GameHUD").gameObject.SetActive(false);
        playerAnimator.SetTrigger("dance");
        foreach (var d in dogsList)
        {
            d.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        endCamera.Priority = 11;
        
        _onRoundEnd?.Invoke();
        ShowScore();
    }

    public void StartShowingTimeScore()
    {
        isShowingTimeScore = true;
    }
    
    public void StartShowingDogsScore()
    {
        isShowingDogsScore = true;
    }
    
    public void StartShowingItemsScore()
    {
        isShowingItemsScore = true;
    }
    
    public void StartShowingTotalScore()
    {
        isShowingTotalScore = true;
    }
    #endregion

    #region DIFICULTY METHODS - CHANGING AMOUNT OF INITIAL DOGS

    const string dificultySaveString = "Dificulty";
    const string countSaveString = "DogCount";
    protected void GetFromSave()
    {
        currentDificulty = Dificulty.Mixed;
        if(PlayerPrefs.HasKey(dificultySaveString))
        {
            currentDificulty = (Dificulty) PlayerPrefs.GetInt(dificultySaveString);
        }
        startingDogsCount = PlayerPrefs.GetInt(countSaveString, 5);

        //Update UI
        dogCountSlider.value = startingDogsCount;
        dificultyDropdown.value = (int) currentDificulty;
    }

    protected void SaveSettings()
    {
        PlayerPrefs.SetInt(dificultySaveString, (int) currentDificulty);
        PlayerPrefs.SetInt(countSaveString, startingDogsCount);
    }

    public void SetDificulty(int id)
    {
        try
        {
            SetDificulty((Dificulty) id);
        }
        catch(Exception e)
        {}
    }

    public void SetDificulty(Dificulty dificulty)
    {
        if(currentDificulty != dificulty )
        {
            currentDificulty = dificulty;
            SpawnDogs(true);            
        }
    }

    public void SetDogCount(float dogCount)
    {
        SetDogCount(Mathf.RoundToInt(dogCount));
    }
    public void SetDogCount(int dogCount)
    {
        if(dogCount != startingDogsCount)
        {
            startingDogsCount = dogCount;
            if(startingDogsCount < 1)
                startingDogsCount = 1;
            
            SpawnDogs(false);
        }
    }

    #endregion

    #region GAME METHODS - STICKS AND TREATS

    public void OnTreatCountChange()
    {
        UpdateTreatsText();
    }

    public void OnStickCountChange()
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
        var totalWeight = dogsList.Select(obj => obj.GetComponent<Dog>().size).Aggregate((x, y) => x + y);
        playerDogsScore = totalWeight * 20;
        playerItemScore = currentSticksCount.Value * 10 + currentTreatsCount.Value * 10;
        playerTimeScore = (int)(100f * (remainingTime / startingTime));
        playerTotalScore = playerTimeScore + playerDogsScore + playerItemScore;

        //setting base score values for the texts
        dogsScoreText.text = "0";
        itemsScoreText.text = "0";
        timeScoreText.text = "0";
        totalScoreText.text = "0";
        
        print("Player scores - Dogs: " + playerDogsScore + " - Items: " + playerItemScore + " - Time: " + playerTimeScore + " - Total: " + playerTotalScore);

        if (playerTotalScore <= 50)
        {
            punResultText.text = "That's was ruff!";
        }
        else if (playerTotalScore <= 100)
        {
            punResultText.text = "Good boy!";
        }
        else
        {
            punResultText.text = "Paw-esome!!!";
        }

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
