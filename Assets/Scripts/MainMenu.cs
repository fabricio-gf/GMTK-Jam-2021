using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuCanvas;

    public void StartRound()
    {
        RoundManager.instance.StartRound();
        mainMenuCanvas.SetActive(false);
    }

    public void PlayCrunchSound()
    {
        AudioManager.instance.GetComponent<EffectsController>().PlayClip("crunch");
    }

    public void PlayClickSound()
    {
        AudioManager.instance.GetComponent<EffectsController>().PlayClip("click");
    }
}
