using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{

    public void OnPause(InputValue input)
    {
        if (input.isPressed)
        {
            RoundManager.instance.TogglePause();
        }
    }
}
