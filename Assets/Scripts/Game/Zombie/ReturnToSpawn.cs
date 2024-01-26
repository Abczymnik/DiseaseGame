using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class ReturnToSpawn : GAction
{
    public override string ActionName { get => "Return to spawn"; }
    public override string ActionType { get => "Static movement"; }
    public override string TargetTag { get => "Spawn point"; }
    public override NavMeshAgent Agent { get; protected set; }

    private Transform playerTrans;

    private new void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        MinRange = 2f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();

        base.Awake();
    }

    private void Start()
    {
        if(playerTrans == null) playerTrans = GameObject.Find("Player").transform;
        Target = GameObject.Find("/SpawnPoints/" + GetTargetSpawnName());
    }

    private WorldState[] SetPreconditions()
    {
        WorldState[] preConditions =
        {
            new WorldState("lost", 1)
        };

        return preConditions;
    }

    private WorldState[] SetAfterEffects()
    {
        WorldState[] afterEffects =
        {
            new WorldState("on spawn", 1)
        };

        return afterEffects;
    }

    public override bool PrePerform()
    {
        Agent.SetDestination(Target.transform.position);
        zombieAnimator.SetBool("move", true);
        return true;
    }

    public override bool PostPerform()
    {
        Beliefs.RemoveState("lost");
        zombieAnimator.SetBool("move", false);
        return true;
    }

    public override bool Func()
    {
        float distToTarget = Vector3.Distance(transform.position, playerTrans.position);
        if (distToTarget >= 12f) { return true; }
        Beliefs.RemoveState("lost");
        return false;
    }

    public override bool Success()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);
        if (distToTarget < MinRange) { return true; }
        return false;
    }

    private string GetTargetSpawnName()
    {
        StringBuilder targetName = new StringBuilder(transform.name);
        targetName.Remove(0, 7);
        targetName.Insert(0, "SpawnPoint ");
        return targetName.ToString();
    }
}
