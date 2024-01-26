using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class LookForPlayer : GAction
{
    public override string ActionName { get => "Look for player"; }
    public override string ActionType { get => "Search"; }
    public override string TargetTag { get => "Player"; }
    public override NavMeshAgent Agent { get; protected set; }

    private new void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        MinRange = 12f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();
        base.Awake();
    }

    private WorldState[] SetPreconditions()
    {
        WorldState[] preConditions =
        {

        };

        return preConditions;
    }

    private WorldState[] SetAfterEffects()
    {
        WorldState[] afterEffects =
        {
            new WorldState("spotted", 1)
        };

        return afterEffects;
    }

    public override bool PrePerform()
    {
        zombieAnimator.SetBool("move", false);
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool Func()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);
        if (distToTarget >= MinRange) return true;
        return false;
    }

    public override bool Success()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);
        if (distToTarget < MinRange && IsTargetInFront(0.3f)) return true;
        if (distToTarget < MinRange / 2) return true;
        return false;
    }
}
