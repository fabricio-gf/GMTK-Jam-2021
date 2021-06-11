using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static AudioSource Source1 = null;
    public static AudioSource Source2 = null;

    // PRIVATE REFERENCES
    [SerializeField] private GameObject SourcePrefab = null;
    private Toggle MusicMuteToggle = null;

    // PUBLIC REFERENCES
    [Space(20)]
    [Header("References")]
    [SerializeField] private AudioClip InitialMusicTrack = null;
    [SerializeField] private float LoopDuration = 0;

    // PRIVATE ATTRIBUTES
    [SerializeField] private float FadeDuration = 0;
    [SerializeField] private float BlendDuration = 0;

    private int CurrentSource = 1;

    private bool IsFading = false;
    private bool IsBlending = false;

    private static string PrefsString = "MusicMute";

    void Awake()
    {
        SceneManager.sceneLoaded += AddListenerToMuteButton;
    }

    private void Start()
    {
        if (Source1 == null && Source2 == null)
        {
            Transform obj = Instantiate(SourcePrefab, new Vector3(0, 0, -10), Quaternion.identity).transform;
            Source1 = obj.GetChild(0).GetComponent<AudioSource>();
            Source2 = obj.GetChild(1).GetComponent<AudioSource>();

            DontDestroyOnLoad(obj);
        }

        Source1.clip = InitialMusicTrack;
        if (!Source1.isPlaying)
        {
            Source1.Play();
            StartCoroutine(LoopTrackAtTime(InitialMusicTrack, Source1, Source2, LoopDuration));
        }

        if (PlayerPrefs.GetInt(PrefsString, 0) == 1)
        {
            Source1.mute = true;
            Source2.mute = true;
        }
    }

    void AddListenerToMuteButton(Scene scene, LoadSceneMode mode)
    {
        MusicMuteToggle = GameObject.Find("MusicMute")?.GetComponent<Toggle>();
        MusicMuteToggle?.onValueChanged.AddListener((bool mute) => ToggleMuteMusic(mute));
    }

    public void ChangeTrackInstantly(AudioClip newTrack, float loopTime)
    {
        if (CurrentSource == 1)
        {
            StopAllCoroutines();
            Source1.clip = newTrack;
            Source1.Play();
            StartCoroutine(LoopTrackAtTime(newTrack, Source2, Source1, loopTime));
        }
        else if (CurrentSource == 2)
        {
            StopAllCoroutines();
            Source2.clip = newTrack;
            Source2.Play();
            StartCoroutine(LoopTrackAtTime(newTrack, Source1, Source2, loopTime));
        }
    }

    public void ChangeTrackBlend(AudioClip newTrack, float loopTime, float BlendDuration)
    {
        if (!IsBlending)
        {
            if (CurrentSource == 1)
            {
                StopAllCoroutines();
                Source2.clip = newTrack;
                Source2.Play();
                StartCoroutine(BlendTracks(Source1, Source2));
                StartCoroutine(LoopTrackAtTime(newTrack, Source1, Source2, loopTime));
                CurrentSource = 2;
            }
            else if (CurrentSource == 2)
            {
                StopAllCoroutines();
                Source1.clip = newTrack;
                Source1.Play();
                StartCoroutine(BlendTracks(Source2, Source1));
                StartCoroutine(LoopTrackAtTime(newTrack, Source2, Source1, loopTime));
                CurrentSource = 1;
            }
        }
    }

    // VOLUME MANIPULATION METHODS

    public void ToggleMuteMusic(bool mute)
    {
        Source1.mute = mute;
        Source2.mute = mute;
        PlayerPrefs.SetInt(PrefsString, mute ? 1 : 0);
    }

    IEnumerator FadeOutTrack(AudioSource Source)
    {
        IsFading = true;
        float time = 0;
        while (time <= FadeDuration)
        {
            Source.volume = 1 - (time / FadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        Source.Stop();
        IsFading = false;
    }

    IEnumerator FadeInTrack(AudioSource Source)
    {
        IsFading = true;
        Source.Play();
        float time = 0;
        while (time <= FadeDuration)
        {
            Source.volume = time / FadeDuration;
            time += Time.deltaTime;
            yield return null;
        }
        IsFading = false;
    }

    IEnumerator BlendTracks(AudioSource FadeOutSource, AudioSource FadeInSource)
    {
        IsBlending = true;
        float time = 0;
        while (time <= BlendDuration)
        {
            FadeOutSource.volume = 1 - (time / BlendDuration);
            FadeInSource.volume = time / BlendDuration;
            time += Time.deltaTime;
            yield return null;
        }
        FadeOutSource.Stop();
        IsBlending = false;
    }

    IEnumerator LoopTrackAtTime(AudioClip clip, AudioSource currentSource, AudioSource newSource, float time)
    {
        yield return new WaitForSeconds(time);
        newSource.clip = clip;
        newSource.Play();
        StartCoroutine(LoopTrackAtTime(clip, newSource, currentSource, time));
    }
}
