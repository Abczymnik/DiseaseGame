using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class BasicZombie : MonoBehaviour
{
    [field: SerializeField, HideInInspector] public Animator ZombieAnimator { get; private set; }
    [field: SerializeField, HideInInspector] public NavMeshAgent ZombieAgent { get; private set; }

    private void OnValidate()
    {
        ZombieAnimator = GetComponent<Animator>();
        ZombieAgent = GetComponent<NavMeshAgent>();
    }
}