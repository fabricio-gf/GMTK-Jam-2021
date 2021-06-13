using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAway : MonoBehaviour
{
    public float flyAwayTime;
    public float flyAwaySpeed;
    private bool hasTriggeredFly = false;
    private bool hasStartedFlying = false;
    public Transform flyDestination;
    public Transform parentTransform;

    private void Update()
    {
        if (!hasStartedFlying) return;
        
        parentTransform.Translate(flyDestination.position * (Time.deltaTime * flyAwaySpeed));
    }

    public void StartFlyAwayDelay()
    {
        if (hasTriggeredFly) return;
        
        hasTriggeredFly = true;
        StartCoroutine(FlyAwayDelay());
    }

    IEnumerator FlyAwayDelay()
    {
        yield return new WaitForSeconds(flyAwayTime);
        hasStartedFlying = true;
        AudioManager.instance.GetComponent<EffectsController>().PlayClip("crow");
    }
}
