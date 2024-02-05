using UnityEngine;

public sealed class CatchPlayer : GAction
{
    public override string ActionName { get => "Catch player"; }
    public override ActionTypes ActionType { get => ActionTypes.DynamicMovement; }
    public override string TargetTag { get => "Player"; }
    [field: SerializeField] public Vector3 ZombieSpawnPoint { get; private set; }

    protected override void Awake()
    {
        ZombieSpawnPoint = transform.position;
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
        ZombieAnimator.SetBool("move", true);
        return true;
    }

    public override bool PostPerform()
    {
        ZombieAnimator.SetBool("move", false);
        return true;
    }

    public override bool Func()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (distToTarget > MaxRange)
        {
            if (IsAgentLost()) { Beliefs.SetState("lost", 1); }

            ZombieAnimator.SetBool("move", false);
            Agent.ResetPath();
            return false;
        }

        Agent.SetDestination(Target.transform.position);
        ZombieAnimator.SetFloat("distanceToPlayer", distToTarget);
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
        if (distToSpawn > 3f) return true;
        return false;
    }
}
