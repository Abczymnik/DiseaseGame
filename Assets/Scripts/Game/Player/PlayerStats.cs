using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private HealthBar playerHealth;
    [SerializeField] private Experience playerExperience;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Animator playerAnimator;

    private UnityAction onPlayerDeath;
    private UnityAction<object> onLevelUp;
    private UnityAction<object> onPlayerDamage;
    private UnityAction<object> onPlayerExperience;

    private Coroutine restoreHealthCoroutine;

    public int PlayerLevel { get; private set; } = 1;
    public float AttackDamage { get; private set; } = 12f;
    public float AttackSpeed { get; private set; } = 1f;
    public float MovementSpeed { get; private set; } = 3.5f;
    public float TimeForHealthRefill { get; private set; } = 15f;
    public bool IsDead { get; private set; } = false;

    public float MaxHealth { get => playerHealth.MaxHealth; private set => playerHealth.MaxHealth = value; }
    public float MaxExp { get => playerExperience.MaxExp; private set => playerExperience.MaxExp = value; }

    public float CurrentHealth
    {
        get { return playerHealth.CurrentHealth; }
        private set
        {
            if(value <= 0)
            {
                playerHealth.CurrentHealth = 0;
                EventManager.TriggerEvent("PlayerDeath");
                return;
            }

            playerHealth.CurrentHealth = value;
        }
    }

    public float CurrentExp
    {
        get { return playerExperience.CurrentExp; }
        private set
        {
            if(value >= MaxExp)
            {
                float playerExpPoints = value;
                int levelsToAdd = 0;

                while(playerExpPoints > MaxExp + levelsToAdd * 10)
                {
                    playerExpPoints -= MaxExp + levelsToAdd * 10;
                    levelsToAdd++;
                }

                PlayerLevel += levelsToAdd;
                EventManager.TriggerEvent("LevelUp", PlayerLevel);
                playerExperience.CurrentExp = playerExpPoints;
                return;
            }

            playerExperience.CurrentExp = value;
        }
    }

    private void Awake()
    {
        if (playerHealth == null) playerHealth = transform.Find("Health orb").GetComponent<HealthBar>();
        if (playerExperience == null) playerExperience = transform.Find("Experience").GetComponent<Experience>();
    }

    private void OnEnable()
    {
        onPlayerDeath += OnPlayerDeath;
        EventManager.StartListening("PlayerDeath", onPlayerDeath);
        onLevelUp += OnLevelUp;
        EventManager.StartListening("LevelUp", onLevelUp);
        onPlayerDamage += OnPlayerDamage;
        EventManager.StartListening("DamagePlayer", onPlayerDamage);
        onPlayerExperience += OnPlayerExperience;
        EventManager.StartListening("AddExperience", onPlayerExperience);
    }

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        MaxExp = 100f;
        CurrentExp = 0f;
    }

    IEnumerator RestoreHealthCoroutine()
    {
        yield return new WaitForSeconds(TimeForHealthRefill);

        while (CurrentHealth < MaxHealth)
        {
            CurrentHealth += 1f * Time.deltaTime;
            yield return null;
        }

        restoreHealthCoroutine = null;
    }

    private void OnPlayerExperience(object experienceData)
    {
        float experienceToAdd = (float)experienceData;
        CurrentExp += experienceToAdd;
    }

    private void OnPlayerDamage(object damageData)
    {
        float damage = (float)damageData;
        CurrentHealth -= damage;
        if (CurrentHealth < 0) { return; }

        if (restoreHealthCoroutine != null) StopCoroutine(restoreHealthCoroutine);

        restoreHealthCoroutine = StartCoroutine(RestoreHealthCoroutine());
    }

    private void OnLevelUp(object playerLevelObj)
    {
        int playerLevel = (int)playerLevelObj;
        MaxHealth = 100 + playerLevel * 10;
        CurrentHealth = MaxHealth;
        MaxExp = 100 * playerLevel * 10;
        AttackDamage = 12f + playerLevel * 2;
        AttackSpeed += 1f + playerLevel * 0.1f;
        MovementSpeed += 3.5f + playerLevel * 0.2f;
    }

    private void OnPlayerDeath()
    {
        IsDead = true;
        playerAnimator.SetBool("dead", true);
        EventManager.StopListening("LevelUp", onLevelUp);
        EventManager.StopListening("DamagePlayer", onPlayerDamage);
        EventManager.StopListening("AddExperience", onPlayerExperience);
        PlayerUI.BlockInput();
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        if (restoreHealthCoroutine != null) StopCoroutine(restoreHealthCoroutine);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", onPlayerDeath);
        EventManager.StopListening("LevelUp", onLevelUp);
        EventManager.StopListening("DamagePlayer", onPlayerDamage);
        EventManager.StopListening("AddExperience", onPlayerExperience);
    }
}


