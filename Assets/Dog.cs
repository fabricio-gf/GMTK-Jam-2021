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

    [SerializeField]
    private Transform rotationPivotTransform;

    [SerializeField]
    private AnimationCurve bounceLoop;

    [SerializeField]
    private AnimationCurve leftRightTiltLoop;

    public float flatAnimTimeMultiplier = 2.5f;
    private float time = 0.0f;

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
        if (Target != null) {
            Target.currentDogs--;
        }
    }

    private void FixedUpdate() {
        // TODO apply force to player based on distraction weight
    }

    private void Update() {
        var dt = flatAnimTimeMultiplier;
        if (Target != null) {
            dt += Target.weight;
        }
        time += dt * Time.deltaTime;

        rotationPivotTransform.localPosition = 0.25f * Vector3.up * bounceLoop.Evaluate(time);
        rotationPivotTransform.localRotation = Quaternion.Euler(20 * (leftRightTiltLoop.Evaluate(time / 2) - 0.5f), 0, 0);

        // var scaledTime = Time.time * 2.5f;
        // rotationPivotTransform.position = 0.25f * Vector3.up * bounceLoop.Evaluate(scaledTime);
        // rotationPivotTransform.rotation = Quaternion.Euler(20 * (leftRightTiltLoop.Evaluate(scaledTime / 2) - 0.5f), 0, 0);
    }
}
