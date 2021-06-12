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

    public Transform leashAttachmentPoint;

    public float distractionForce;
    public float flatAnimTimeMultiplier = 2.5f;

    private Rigidbody rb;
    private Transform playerTransform;
    private Transform playerHipTransform;
    private new Transform transform;

    [SerializeField]
    private Transform rotationPivotTransform;

    [SerializeField]
    private AnimationCurve bounceLoop;

    [SerializeField]
    private AnimationCurve leftRightTiltLoop;

    private LineRenderer leashRenderer;

    private float time;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        time = Random.value;
        transform = GetComponent<Transform>();
        leashRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        playerTransform = GetComponent<SpringJoint>().connectedBody.transform;
        playerHipTransform = playerTransform.Find("Skeleton").Find("Hips");
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

        Transform lookTarget;
        if (Target == null) {
            lookTarget = playerTransform;
        } else {
            lookTarget = targetTransform;
        }
        var lookDir = lookTarget.position - transform.position;
        var rot = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    private void LateUpdate() {
        leashRenderer.SetPositions(new Vector3[] {leashAttachmentPoint.position, playerHipTransform.position});
    }
}
