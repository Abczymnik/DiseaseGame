using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SpecialZombie : MonoBehaviour
{
    private Animator zombieAnimator;
    private NavMeshAgent zombieAgent;
    private UnityAction onLevelChange;

    private void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        onLevelChange += OnLevelChange;
        EventManager.StartListening("ChangeLevel", onLevelChange);
    }

    private void OnLevelChange()
    {
        zombieAnimator.SetBool("move", true);
        zombieAnimator.SetFloat("distanceToPlayer", 6f);
        zombieAgent.SetDestination(new Vector3(32, 2, 0));
    }

    private void OnDisable()
    {
        EventManager.StopListening("ChangeLevel", onLevelChange);
    }
}