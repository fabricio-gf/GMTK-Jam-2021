using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string mainLevelName;
    
    public void LoadMainLevel()
    {
        SceneManager.LoadScene(mainLevelName);
    }
}
