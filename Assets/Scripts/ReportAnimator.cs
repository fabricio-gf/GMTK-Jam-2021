using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportAnimator : MonoBehaviour
{
    public void StartTimeCounter()
    {
        RoundManager.instance.StartShowingTimeScore();
    }

    public void StartDogsCounter()
    {
        RoundManager.instance.StartShowingDogsScore();
    }

    public void StartItemsCounter()
    {
        RoundManager.instance.StartShowingItemsScore();
    }

    public void StartTotalCounter()
    {
        RoundManager.instance.StartShowingTotalScore();
    }

    public void PlayStampEffect()
    {
        //AudioManager.instance.GetComponentInChildren<EffectsController>().PlayClip("stamp");
    }

    public void PlayResultEffect()
    {
        //AudioManager.instance.GetComponentInChildren<EffectsController>().PlayClip("result");
    }
}
