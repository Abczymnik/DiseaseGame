using System.Collections;
using UnityEngine;

public sealed class EnemyAttack : GAction
{
    public override string ActionName { get => "Attack player"; }
    public override ActionTypes ActionType { get => ActionTypes.Attack; }
    public override string TargetTag { get => "Player"; }

    [SerializeField] private float zombieAttackDamage;
    private PlayerStats playerStats;
    private Coroutine attackPlayerCoroutine;

    protected override void Awake()
    {
        MaxRange = 1.2f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();
        base.Awake();
    }

    protected override void OnValidate()
    {
        zombieAttackDamage = GetComponent<Zombie>().AttackDamage;

        base.OnValidate();
    }

    private void Start()
    {
        playerStats = Target.GetComponent<PlayerStats>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private WorldState[] SetPreconditions()
    {
        WorldState[] preConditions =
        {
            new WorldState("caught", 1)
        };

        return preConditions;
    }

    private WorldState[] SetAfterEffects()
    {
        WorldState[] afterEffects =
        {
            new WorldState("kill player", 1)
        };

        return afterEffects;
    }

    public override bool PrePerform()
    {
        ZombieAnimator.SetBool("attack", true);
        if (attackPlayerCoroutine == null && IsTargetInFront(0.75f)) attackPlayerCoroutine = StartCoroutine(ZombieAttack());
        return true;
    }

    public override bool PostPerform()
    {
        if (attackPlayerCoroutine != null)
        {
            StopCoroutine(attackPlayerCoroutine);
            attackPlayerCoroutine = null;
        }
        ZombieAnimator.SetBool("attack", false);
        return true;
    }

    public override bool Func()
    {
        float distToTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (distToTarget > MaxRange || !IsTargetInFront(0.75f))
        {
            if (attackPlayerCoroutine != null)
            {
                StopCoroutine(attackPlayerCoroutine);
                attackPlayerCoroutine = null;
            }

            ZombieAnimator.SetBool("attack", false);
            return false;
        }
        return true;
    }

    public override bool Success()
    {
        if(playerStats.IsDead)
        {
            ZombieAnimator.SetBool("playerIsDead", true);
            ZombieAnimator.SetBool("attack", false);
            return true;
        }
        return false;
    }

    IEnumerator ZombieAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            EventManager.TriggerEvent(TypedEventName.DamagePlayer, zombieAttackDamage);
            yield return new WaitForSeconds(0.5f);
            EventManager.TriggerEvent(TypedEventName.DamagePlayer, zombieAttackDamage);
        }
    }
}
