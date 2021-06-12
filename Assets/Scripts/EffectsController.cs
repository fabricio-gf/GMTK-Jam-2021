using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EffectsController : MonoBehaviour
{
    public static AudioSource Source = null;

    // PUBLIC REFERENCES

    // PRIVATE REFERENCES
    [Header("References")]
    [SerializeField] private GameObject SourcePrefab = null;

    private Toggle SFXMuteToggle = null;

    [Header("Clips")]
    [SerializeField] private AudioClip[] ClipList = null;
    [SerializeField] private string[] TagList = null;

    // PRIVATE ATTRIBUTES
    private Dictionary<string, AudioClip> Clips = null;

    private static string PrefsString = "SFXMute";

    void Awake()
    {
        Clips = new Dictionary<string, AudioClip>();
        FillClips();
        SceneManager.sceneLoaded += AddListenerToMuteButton;
    }

    private void Start()
    {
        if (Source == null)
        {
            GameObject obj = Instantiate(SourcePrefab, new Vector3(0, 0, -10), Quaternion.identity);
            Source = obj.GetComponent<AudioSource>();

            DontDestroyOnLoad(obj);

        }

        if (PlayerPrefs.GetInt(PrefsString, 0) == 1)
        {
            Source.mute = true;
        }
    }

    void AddListenerToMuteButton(Scene scene, LoadSceneMode mode)
    {
        SFXMuteToggle = GameObject.Find("EffectsMute")?.GetComponent<Toggle>();
        SFXMuteToggle?.onValueChanged.AddListener((bool mute) => ToggleMuteSFX(mute));
    }

    void FillClips()
    {
        for(int i = 0; i < ClipList.Length; i++)
        {
            if(i == TagList.Length)
            {
                return;
            }
            Clips.Add(TagList[i], ClipList[i]);
        }
    }

    public void PlayClip(string key)
    {
        AudioClip clip;
        if (Clips.TryGetValue(key, out clip))
        {
            Source.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No clip found! Try another name");
        }
    }

    public void ToggleMuteSFX(bool mute)
    {
        Source.mute = mute;
        PlayerPrefs.SetInt(PrefsString, mute ? 1 : 0);
    }
}
