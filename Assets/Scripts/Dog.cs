using UnityEngine;

public class Dog : MonoBehaviour 
{
    // TODO possibly use a priority queue to allow backup distractions
    private Distraction _target;
    private Transform targetTransform;
    public Distraction Target 
    {
        get { return _target; }
        set {
            if (!hasRoundStarted) return;
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

    // Idle Behaviour
    private float idleTime = 0f;
    private float maxIdleTime = 5f;
    private bool isWandering = false;
    private float wanderChance = 0.5f;
    private Vector2 currentWanderingDirection;

    // Round start
    private bool hasRoundStarted = false;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        time = Random.value;
        transform = GetComponent<Transform>();
        leashRenderer = GetComponent<LineRenderer>();
    }

    private void Start() 
    {
        playerTransform = GetComponent<SpringJoint>().connectedBody.transform;
        playerHipTransform = playerTransform.Find("Skeleton").Find("Hips");

        maxIdleTime += Random.value - 0.5f;
    }

    private void EnableBehaviour()
    {
        hasRoundStarted = true;
    }

    private void FocusNewTarget() 
    {
        if (Target == null)
        {
            IdleBehaviour();
        }
        else {
            Target.currentDogs++;
        }
    }

    private void IdleBehaviour()
    {
        idleTime = 0f;
        isWandering = (Random.value <= wanderChance);
        if (isWandering)
        {
            currentWanderingDirection = Random.insideUnitCircle;
        }
    }

    private void ReduceTargetDogCount() 
    {
        if (Target != null) {
            Target.currentDogs--;
        }
    }

    private void FixedUpdate() 
    {
        if (Target != null) 
        {
            rb.AddForce(transform.forward * (Target.weight * distractionForce), ForceMode.Force);
        }
        else
        {
            if (isWandering)
            {
                rb.AddForce(transform.forward * (1f * distractionForce), ForceMode.Force);
            }
        }
    }

    private void Update()
    {
        var dt = flatAnimTimeMultiplier;
        if (Target != null)
        {
            dt += Target.weight;
        }
        else
        {
            idleTime += Time.deltaTime;
            if(idleTime >= maxIdleTime)
            {
                IdleBehaviour();
            }
        }
        time += dt * Time.deltaTime;

        rotationPivotTransform.localPosition = Vector3.up * (0.25f * bounceLoop.Evaluate(time));
        rotationPivotTransform.localRotation = Quaternion.Euler(0, 0, 20 * (leftRightTiltLoop.Evaluate(time / 2) - 0.5f));
        
        CalculateLookDirection();
    }

    private void CalculateLookDirection()
    {
        Vector3 lookTarget;
        if (Target == null)
        {
            const float distance = 10f;
            lookTarget = isWandering ?
                transform.position + new Vector3(currentWanderingDirection.x, 0f, currentWanderingDirection.y) * distance :
                playerTransform.position;
        }
        else
        {
            lookTarget = targetTransform.position;
        }
        var lookDir = lookTarget - transform.position;
        var rot = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    private void LateUpdate() 
    {
        leashRenderer.SetPositions(new Vector3[] {leashAttachmentPoint.position, playerHipTransform.position});
    }

    private void OnEnable()
    {
        RoundManager.instance._onRoundStart += EnableBehaviour;
    }

    private void OnDisable()
    {
        RoundManager.instance._onRoundStart -= EnableBehaviour;
    }
}
