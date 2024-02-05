using UnityEngine;

public sealed class ReturnToSpawn : GAction
{
    public override string ActionName { get => "Return to spawn"; }
    public override ActionTypes ActionType { get => ActionTypes.StaticMovement; }
    public override string TargetTag { get => "Spawn point"; }
    [field: SerializeField] public Vector3 ZombieSpawnPoint { get; private set; }

    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform spawnerTrans;
    private GameObject spawnPoint;

    protected override void Awake()
    {
        ZombieSpawnPoint = transform.position;
        MaxRange = 12f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();

        base.Awake();
    }

    protected override void OnValidate()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        spawnerTrans = GameObject.FindGameObjectWithTag("Respawn").transform;

        base.OnValidate();
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
        if (spawnPoint == null)
        {
            CreateSpawnPoint();
            Target = spawnPoint;
        }

        Agent.SetDestination(Target.transform.position);
        ZombieAnimator.SetBool("move", true);
        return true;
    }

    public override bool PostPerform()
    {
        Beliefs.RemoveState("lost");
        ZombieAnimator.SetBool("move", false);
        return true;
    }

    public override bool Func()
    {
        if (IsPlayerNearby())
        {
            Beliefs.RemoveState("lost");
            return false;
        }
        return true;
    }

    public override bool Success()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);
        if (distToTarget < MinRange) { return true; }
        return false;
    }

    private bool IsPlayerNearby()
    {
        float distToPlayer = Vector3.Distance(transform.position, playerTrans.transform.position);
        if (distToPlayer < MaxRange && IsPlayerInFront(0.3f)) return true;
        if (distToPlayer < MaxRange / 2) return true;
        return false;
    }

    private bool IsPlayerInFront(float range)
    {
        Vector3 dirToPlayer = (playerTrans.transform.position - transform.position).normalized;
        bool playerInFront = Vector3.Dot(transform.forward, dirToPlayer) > range;
        return playerInFront;
    }

    private void CreateSpawnPoint()
    {
        spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.position = ZombieSpawnPoint;
        spawnPoint.transform.SetParent(spawnerTrans);
    }
}
