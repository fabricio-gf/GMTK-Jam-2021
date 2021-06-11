using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    // TODO possibly use a priority queue to allow backup distractions
    private Distraction _target;
    public Distraction Target {
        get { return _target; }
        set {
            ReduceTargetDogCount();
            _target = value;
            FocusNewTarget();
        }
    }

    // TODO
    // public GameObject player;
    // public float distanceFromPlayer;

    private void FocusNewTarget() {
        if (Target == null) {
            // TODO idle movement
        } else {
            Target.currentDogs++;
            // TODO rotate around player to focus new target
        }
    }

    private void ReduceTargetDogCount() {
        if (Target != null){
            Target.currentDogs--;
        }
    }

    private void FixedUpdate() {
        // TODO apply force to player based on distraction weight
    }
}
