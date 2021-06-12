using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEnabler : MonoBehaviour {
    void Start() {
        RoundManager.instance._onRoundStart += EnablePlayerInput;
    }

    void EnablePlayerInput() {
        RoundManager.instance._onRoundStart -= EnablePlayerInput;
        GetComponent<PlayerInput>().enabled = true;
    }
}
