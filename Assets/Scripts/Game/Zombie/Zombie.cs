using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Zombie : MonoBehaviour
{
    [SerializeField] private HealthBar zombieHealthBar;
    [SerializeField] private GameObject loot;
    [SerializeField] private Animator zombieAnimator;

    private UnityAction<object> onZombieDeath;

    [SerializeField] private float experiencePoints = 40f;
    [SerializeField] private float dropChance = 1f;

    public float AttackDamage { get; private set; } = 5;
    public bool IsDead { get; private set; }

    public float CurrentHealth
    {
        get { return zombieHealthBar.CurrentHealth; }
        set
        {
            if (value <= 0)
            {
                zombieHealthBar.CurrentHealth = 0;
                EventManager.TriggerEvent("ZombieDeath", this.GetInstanceID());
                return;
            }

            zombieHealthBar.CurrentHealth = value;
        }
    }

    private void OnValidate()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieHealthBar = transform.GetChild(3).GetComponent<HealthBar>();
        loot = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        onZombieDeath += OnDeath;
        EventManager.StartListening("ZombieDeath", onZombieDeath);
    }

    public void TakeDamage(float hit)
    {
        CurrentHealth -= hit;
    }    

    private void DropItem(float chance)
    {
        float drop = Random.Range(0, 1f);
        if (drop > chance) return;
        loot.SetActive(true);
    }

    private void OnDeath(object zombieID)
    {
        if (GetInstanceID() != (int)zombieID) return;

        IsDead = true;
        EventManager.TriggerEvent("AddExperience", experiencePoints);
        zombieAnimator.SetBool("attack", false);
        zombieAnimator.SetInteger("deadType", Random.Range(0, 2));
        zombieAnimator.SetBool("dead", true);

        GetComponent<Collider>().enabled = false;
        GetComponent<RootMotion>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<ReturnToSpawn>().enabled = false;
        GetComponent<CatchPlayer>().enabled = false;
        GetComponent<LookForPlayer>().enabled = false;
        GetComponent<ZombieGoals>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<GAgent>().enabled = false;
        transform.GetChild(3).gameObject.SetActive(false);
        DropItem(dropChance);
    }

    private void OnDisable()
    {
        EventManager.StopListening("ZombieDeath", onZombieDeath);
    }
}
