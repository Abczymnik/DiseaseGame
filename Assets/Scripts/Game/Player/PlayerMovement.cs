using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(PlayerStats), typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private const float ACCELERATION_TIME = 0.35f;
    private const float MOVEMENT_REFRESH_RATE = 0.1f;
    private const float GRAVITY_FORCE = 9.8f;
    private const float SOFT_GRAVITY_PULL = 0.098f;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private PlayerStats playerStats;

    private InputAction moveInput;
    private UnityAction<object> onLevelUp;

    private float currentMinMove;
    private float timeToReachTarget;
    private float currentRunTime;
    private float timeToAccelerate;
    private float timeForFullSpeed;
    private float timeToDecelerate;
    private float timeToAccelerateDisplacement;
    private float timeToDecelerateDisplacement;
    private Vector3 targetWorldPosition;
    private Coroutine moveCoroutine;

    private Coroutine rotateCoroutine;

    public float MaxVelocity { get; private set; }
    public float Acceleration { get; private set; }
    public float RotationSpeed { get; private set; }

    private bool _isRunning;
    public bool IsRunning
    {
        get { return _isRunning; }
        private set
        {
            _isRunning = value;
            if (!value) Velocity = 0f; 
            currentRunTime = 0f;
        }
    }

    private float _velocity;
    public float Velocity
    {
        get { return _velocity; }
        private set
        {
            _velocity = value;
            playerAnimator.SetFloat("Velocity", _velocity);
        }
    }

    private void OnValidate()
    {
        playerStats = GetComponent<PlayerStats>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        onLevelUp += OnLevelUp;
        EventManager.StartListening("LevelUp", onLevelUp);
    }

    private void Awake()
    {
        moveInput = PlayerUI.Instance.InputActions.Gameplay.Move;
        MovementUIOn();
    }

    private void Start()
    {
        MaxVelocity = playerStats.MovementSpeed;
        Acceleration = MaxVelocity / ACCELERATION_TIME;
        RotationSpeed = playerStats.RotationSpeed;
        currentMinMove = 0.5f/RotationSpeed;
    }

    private void Update()
    {
        Movement();  
    }

    private void MapInputToWorld(InputAction.CallbackContext context)
    {
        Vector2 inputVector;

        if (context.control.device.displayName == "Mouse")
        {
            if (UIHelper.IsPointerOverUI()) return;

            inputVector = moveInput.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(inputVector);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                targetWorldPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                float tempDistToTarget = Vector3.Distance(transform.position, targetWorldPosition);
                if (ActualMinMoveDist() < tempDistToTarget) CalcMovementTimers();
            }
        }

        else if (moveCoroutine == null) moveCoroutine = StartCoroutine(KeyboardMoveCoroutine());

        IEnumerator KeyboardMoveCoroutine()
        {
            while (moveInput.IsPressed())
            {
                inputVector = moveInput.ReadValue<Vector2>();
                Vector3 inputDirection = ConvertInputToIsometric(inputVector);
                float tempDistToTarget = ActualMinMoveDist();

                targetWorldPosition = transform.position + tempDistToTarget * MaxVelocity * inputDirection;
                CalcMovementTimers();
                yield return new WaitForSeconds(MOVEMENT_REFRESH_RATE);
            }
            moveCoroutine = null;
        }
    }

    private float ActualMinMoveDist()
    {
        float tempMinMove = Velocity * 0.5f * Velocity / Acceleration;
        if (tempMinMove > currentMinMove)
        {
            return tempMinMove;
        }
        return currentMinMove;
    }

    private void CalcMovementTimers()
    {
        CalculateRotationDir();
        IsRunning = true;
        float distToTarget = Vector3.Distance(targetWorldPosition, transform.position);
        timeToAccelerate = (-Velocity + Mathf.Sqrt(Velocity * Velocity + 2 * Acceleration * distToTarget)) / Acceleration;

        float vMaxInStage = Velocity + Acceleration * timeToAccelerate;
        if (vMaxInStage <= MaxVelocity)
        {
            timeForFullSpeed = 0f;
            timeToDecelerate = vMaxInStage / Acceleration;
        }

        else
        {
            timeToAccelerate = (MaxVelocity - Velocity) / Acceleration;
            timeToDecelerate = ACCELERATION_TIME;
            float inStageDist = (MaxVelocity + Velocity) * 0.5f * timeToAccelerate;
            float outStageDist = 0.5f * Acceleration * ACCELERATION_TIME * ACCELERATION_TIME;
            timeForFullSpeed = (distToTarget - inStageDist - outStageDist) / MaxVelocity;
        }

        timeToDecelerateDisplacement = ACCELERATION_TIME - timeToDecelerate;
        timeToAccelerateDisplacement = Velocity / Acceleration;
        timeToReachTarget = timeToAccelerate + timeForFullSpeed + timeToDecelerate;
    }

    private Vector3 ConvertInputToIsometric(Vector2 inputVector)
    {
        Vector3 toConvert = new Vector3(inputVector.x, 0, inputVector.y);
        Quaternion isometricRotation = Quaternion.Euler(0, 45.0f, 0);
        Matrix4x4 isometricMatrix = Matrix4x4.Rotate(isometricRotation);
        return isometricMatrix.MultiplyPoint3x4(toConvert);
    }

    private void Movement()
    {
        if (!IsRunning)
        {
            if (playerController.isGrounded) return;

            playerController.Move(Time.deltaTime * GRAVITY_FORCE * Vector3.down);
            return;
        }

        currentRunTime += Time.deltaTime;
        if (currentRunTime > timeToReachTarget)
        {
            IsRunning = false;
            return;
        }

        CalcActualVelocity(currentRunTime);

        Vector3 motionXZAxis = Velocity * transform.forward;
        Vector3 motionYAxis = playerController.isGrounded ? Vector3.down * SOFT_GRAVITY_PULL : Vector3.down * GRAVITY_FORCE;
        Vector3 completeMotion = motionXZAxis + motionYAxis;
        playerController.Move(Time.deltaTime * completeMotion);
    }

    private void CalcActualVelocity(float t)
    {
        if (currentRunTime <= timeToAccelerate)
        {
            Velocity = QuadEaseIn(t + timeToAccelerateDisplacement);
            return;
        }

        if (currentRunTime > timeToAccelerate + timeForFullSpeed)
        {
            Velocity = QuadEaseOut(t + timeToDecelerateDisplacement);
            return;
        }

        Velocity = MaxVelocity;
    }

    private float QuadEaseIn(float t) => Acceleration * t;

    private float QuadEaseOut(float t)
    {
        t -= timeToAccelerate + timeForFullSpeed;
        return MaxVelocity - (Acceleration * t);
    }

    private void CalculateRotationDir()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetWorldPosition - transform.position);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(PerformRotation(targetRotation));
    }

    private IEnumerator PerformRotation(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        float rotationPercentage = 0f;

        while (rotationPercentage < 1)
        {
            rotationPercentage += Time.deltaTime * RotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationPercentage);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void StopMovementForSeconds(float seconds)
    {
        if (Velocity > 0)
        {
            float minMoveDist = ActualMinMoveDist();
            targetWorldPosition = transform.position + minMoveDist * transform.forward;
            CalcMovementTimers();
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        StartCoroutine(WaitForStop());

        IEnumerator WaitForStop()
        {
            while(Velocity > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(seconds);
        }
    }

    private void OnLevelUp(object level)
    {
        MaxVelocity = playerStats.MovementSpeed;
        Acceleration = MaxVelocity / ACCELERATION_TIME;
    }

    public void MovementUIOn()
    {
        moveInput.performed += MapInputToWorld;
    }

    public void MovementUIOff()
    {
        moveInput.performed -= MapInputToWorld;
    }

    private void OnDisable()
    {
        MovementUIOff();
        EventManager.StopListening("LevelUp", onLevelUp);
    }
}