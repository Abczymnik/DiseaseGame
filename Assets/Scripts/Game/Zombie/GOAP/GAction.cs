using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public abstract string ActionName { get; }
    public abstract string ActionType { get; }
    public abstract string TargetTag { get; }
    public abstract NavMeshAgent Agent { get; protected set; }

    [field: SerializeField] public GameObject Target { get; protected set; }
    [field: SerializeField] public float Cost { get; protected set; } = 1f;
    [field: SerializeField] public float MinRange { get; protected set; } = 1.5f;
    [field: SerializeField] public float MaxRange { get; protected set; } = 1.5f;
    [field: SerializeField] protected WorldState[] PreConditionsVisual { get; set; }
    [field: SerializeField] protected WorldState[] AfterEffectsVisual { get; set; }
    [field: SerializeField] public WorldStates Beliefs { get; protected set; }

    public Dictionary<string, int> Preconditions { get; set; }
    public Dictionary<string, int> Effects { get; set; }

    protected Animator zombieAnimator;
    public Vector3 ZombieSpawnPoint { get; private set; }
    public bool running = false;

    public void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
        Preconditions = new Dictionary<string, int>();
        Effects = new Dictionary<string, int>();
        if (Target == null) Target = GameObject.FindGameObjectWithTag(TargetTag);
        UpdatePreAfterEff();

        ZombieSpawnPoint = transform.position;
        Beliefs = GetComponent<GAgent>().beliefs;
    }

    protected void UpdatePreAfterEff()
    {
        foreach (WorldState w in PreConditionsVisual)
        {
            Preconditions.Add(w.Key, w.Value);
        }

        foreach (WorldState w in AfterEffectsVisual)
        {
            Effects.Add(w.Key, w.Value);
        }
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in Preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

    protected bool IsTargetInFront(float range)
    {
        Vector3 dirToTarget = (Target.transform.position - transform.position).normalized;
        bool targetInFront = Vector3.Dot(transform.forward, dirToTarget) > range;
        return targetInFront;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
    public abstract bool Func();
    public abstract bool Success();
}
