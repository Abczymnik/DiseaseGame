using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RootMotion : MonoBehaviour
{
    private Animator zombieAnimator;
    private GAction activeGAction;
    private Coroutine rotationCoroutine;
    private ZombieGoals zombieGoals;
    private NavMeshAgent agent;

    private Vector2 zombieVelocity;
    private Vector2 smoothDeltaPosition;

    private void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieGoals = GetComponent<ZombieGoals>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (zombieAnimator == null) return;
        activeGAction = zombieGoals.CurrentAction;
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    private void Update()
    {
        if (activeGAction == null || activeGAction.running == false)
        {
            activeGAction = zombieGoals.CurrentAction;
            return;
        }

        float distToTarget = Vector3.Distance(transform.position, activeGAction.Target.transform.position);
        if (distToTarget < 1.2f)
        {
            if (IsTargetInFront(0.9f)) return;

            if (rotationCoroutine == null) rotationCoroutine = StartCoroutine(PerformRotation(distToTarget));
        }

        if(zombieAnimator.GetBool("move") == true) SynchronizeAnimatorWithAgent();
    }

    private void SynchronizeAnimatorWithAgent()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0f;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        zombieVelocity = smoothDeltaPosition / Time.deltaTime;
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            zombieVelocity = Vector2.Lerp(Vector2.zero, zombieVelocity, agent.remainingDistance / agent.stoppingDistance);
        }

        if (zombieVelocity.magnitude > 0.5f) { zombieAnimator.SetFloat("velocity", zombieVelocity.magnitude); }

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if(deltaMagnitude > agent.radius / 2f)
        {
            transform.position = Vector3.Lerp(zombieAnimator.rootPosition, agent.nextPosition, smooth);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = zombieAnimator.rootPosition;
        rootPosition.y = agent.nextPosition.y;
        transform.position = rootPosition;
        agent.nextPosition = rootPosition;
    }

    IEnumerator PerformRotation(float rotationSpeedDenom)
    {
        float rotationSpeed = 5 / rotationSpeedDenom;
        float rotationProgress = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(activeGAction.Target.transform.position - transform.position);
        endRotation.x = transform.rotation.x;

        while (true)
        {
            if (rotationProgress > 1) break;

            rotationProgress += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, rotationProgress);
            yield return null;
        }

        rotationCoroutine = null;
    }

    private bool IsTargetInFront(float range)
    {
        Vector3 dirToTarget = (activeGAction.Target.transform.position - transform.position).normalized;
        bool targetInFront = Vector3.Dot(transform.forward, dirToTarget) > range;
        return targetInFront;
    }
}
