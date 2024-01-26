using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Zombie : MonoBehaviour
{
    [SerializeField] private HealthBar zombieHealthBar;
    [SerializeField] private GameObject note;
    private Animator zombieAnimator;

    private UnityAction<object> onZombieDeath;

    private float experiencePoints = 40f;

    public float DropChance { get; private set; } = 1f;
    public float AttackDamage { get; private set; } = 10;
    public bool IsDead { get; private set; }
    public float MaxHealth { get => zombieHealthBar.MaxHealth; private set => zombieHealthBar.MaxHealth = value; }

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

    private void OnEnable()
    {
        onZombieDeath += OnDeath;
        EventManager.StartListening("ZombieDeath", onZombieDeath);
    }

    private void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(zombieHealthBar == null) zombieHealthBar = transform.GetChild(2).GetChild(0).GetComponent<HealthBar>();
        if(note == null) note = transform.GetChild(0).GetChild(0).gameObject;
    }

    public void TakeDamage(float hit)
    {
        CurrentHealth -= hit;
    }    

    private void DropItem(float chance)
    {
        float drop = Random.Range(0, 1f);
        if (drop > chance) return;
        EventManager.TriggerEvent("NewNoteOnMap");
        note.SetActive(true);
    }

    private void OnDeath(object zombieID)
    {
        if (this.GetInstanceID() != (int)zombieID) return;

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
        transform.GetChild(2).gameObject.SetActive(false);
        DropItem(DropChance);
    }

    private void OnDisable()
    {
        EventManager.StopListening("ZombieDeath", onZombieDeath);
    }
}
