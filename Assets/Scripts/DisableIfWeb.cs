using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableIfWeb : MonoBehaviour
{
    #if UNITY_WEBGL
    void Start()
    {
        Button button = GetComponent<Button>();
        if(button != null)
            button.interactable = false;
        else
            gameObject.SetActive(false);
    }
    #endif
}