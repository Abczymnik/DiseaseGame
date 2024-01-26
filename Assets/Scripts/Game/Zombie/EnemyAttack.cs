using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : GAction
{
    public override string ActionName { get => "Attack player"; }
    public override string ActionType { get => "Attack"; }
    public override string TargetTag { get => "Player"; }
    public override NavMeshAgent Agent { get; protected set; }

    private PlayerStats playerStats;
    private Coroutine attackPlayerCoroutine;

    private new void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        MaxRange = 1.2f;
        PreConditionsVisual = SetPreconditions();
        AfterEffectsVisual = SetAfterEffects();
        base.Awake();
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
        zombieAnimator.SetBool("attack", true);
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
        zombieAnimator.SetBool("attack", false);
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

            zombieAnimator.SetBool("attack", false);
            return false;
        }
        return true;
    }

    public override bool Success()
    {
        if(playerStats.IsDead)
        {
            zombieAnimator.SetBool("playerIsDead", true);
            zombieAnimator.SetBool("attack", false);
            return true;
        }
        return false;
    }

    IEnumerator ZombieAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            EventManager.TriggerEvent("DamagePlayer", 5f);
            yield return new WaitForSeconds(0.5f);
            EventManager.TriggerEvent("DamagePlayer", 5f);
        }
    }
}
