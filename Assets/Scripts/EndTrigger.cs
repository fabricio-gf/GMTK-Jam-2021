using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Enter End Trigger");
        if (other.CompareTag("Player"))
        {
            print("Collide with player");
            RoundManager.instance.RoundEndVictory();
        }
    }
}
