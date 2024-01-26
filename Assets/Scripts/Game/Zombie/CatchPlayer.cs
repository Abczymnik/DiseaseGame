using UnityEngine;
using UnityEngine.AI;

public sealed class CatchPlayer : GAction
{
    public override string ActionName { get => "Catch player"; }
    public override string ActionType { get => "Dynamic movement"; }
    public override string TargetTag { get => "Player"; }
    public override NavMeshAgent Agent { get; protected set; }

    private new void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        MaxRange = 12f;
        MinRange = 1.2f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();
        base.Awake();
    }

    private WorldState[] SetPreconditions()
    {
        WorldState[] preConditions =
        {
            new WorldState("spotted",1)
        };

        return preConditions;
    }

    private WorldState[] SetAfterEffects()
    {
        WorldState[] afterEffects =
        {
            new WorldState("caught",1)
        };

        return afterEffects;
    }

    public override bool PrePerform()
    {
        zombieAnimator.SetBool("move", true);
        return true;
    }

    public override bool PostPerform()
    {
        zombieAnimator.SetBool("move", false);
        return true;
    }

    public override bool Func()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (distToTarget > MaxRange)
        {
            if (IsAgentLost()) { Beliefs.ModifyState("lost", 1); }

            zombieAnimator.SetBool("move", false);
            Agent.ResetPath();
            return false;
        }

        Agent.SetDestination(Target.transform.position);
        zombieAnimator.SetFloat("distanceToPlayer", distToTarget);
        return true;
    }

    public override bool Success()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);
        if (distToTarget < MinRange)
        {
            Agent.ResetPath();
            return true;
        }
        return false;
    }

    private bool IsAgentLost()
    {
        float distToSpawn = Vector3.Distance(transform.position, ZombieSpawnPoint);
        if (distToSpawn > 5f) return true;
        return false;
    }
}
