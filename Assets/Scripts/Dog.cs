using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {
    // TODO possibly use a priority queue to allow backup distractions
    private Distraction _target;
    private Transform targetTransform;
    public Distraction Target {
        get { return _target; }
        set {
            ReduceTargetDogCount();
            _target = value;
            targetTransform = _target?.transform;
            FocusNewTarget();
        }
    }

    public float distractionForce = 400.0f;

    private Rigidbody rb;

    [SerializeField]
    private Transform rotationPivotTransform;

    [SerializeField]
    private AnimationCurve bounceLoop;

    [SerializeField]
    private AnimationCurve leftRightTiltLoop;

    public float flatAnimTimeMultiplier = 2.5f;
    private float time = 0.0f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FocusNewTarget() {
        if (Target == null) {
            // TODO idle movement
        } else {
            Target.currentDogs++;
        }
    }

    private void ReduceTargetDogCount() {
        if (Target != null) {
            Target.currentDogs--;
        }
    }

    private void FixedUpdate() {
        if (Target != null) {
            rb.AddForce(transform.forward * Target.weight * distractionForce, ForceMode.Force);
        }
    }

    private void Update() {
        var dt = flatAnimTimeMultiplier;
        if (Target != null) {
            dt += Target.weight;
        }
        time += dt * Time.deltaTime;

        rotationPivotTransform.localPosition = 0.25f * Vector3.up * bounceLoop.Evaluate(time);
        rotationPivotTransform.localRotation = Quaternion.Euler(0, 0, 20 * (leftRightTiltLoop.Evaluate(time / 2) - 0.5f));

        if (Target != null) {
            var dir = targetTransform.position - transform.position;
            var rot = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rot, 0);
        }
    }
}
