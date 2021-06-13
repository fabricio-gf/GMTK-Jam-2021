using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public AudioClip gameMusic;
    
    public void StartRound()
    {
        RoundManager.instance.StartRound();
        AudioManager.instance.GetComponent<MusicController>().ChangeTrackInstantly(gameMusic, 138.182f);
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
