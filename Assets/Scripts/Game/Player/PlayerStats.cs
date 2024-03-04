using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(PlayerMovement), typeof(PlayerAttack))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private HealthBar playerHealth;
    [SerializeField] private Experience playerExperience;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Animator playerAnimator;

    private UnityAction onPlayerDeath;
    private UnityAction<object> onLevelUp;
    private UnityAction<object> onPlayerDamage;
    private UnityAction<object> onPlayerExperience;

    private Coroutine restoreHealthCoroutine;

    public int PlayerLevel { get; private set; } = 0;
    public float AttackDamage { get; private set; } = 12f;
    public float AttackSpeed { get; private set; } = 1f;
    public float MovementSpeed { get; private set; } = 3.5f;
    public float RotationSpeed { get; private set; } = 6f;
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
                EventManager.TriggerEvent(UnityEventName.PlayerDeath);
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
                EventManager.TriggerEvent(TypedEventName.LevelUp, PlayerLevel);
                playerExperience.CurrentExp = playerExpPoints;
                return;
            }

            playerExperience.CurrentExp = value;
        }
    }

    private void OnValidate()
    {
        playerHealth = transform.Find("Health orb").GetComponent<HealthBar>();
        playerExperience = transform.Find("Experience").GetComponent<Experience>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        SetUpEvents();
    }

    private void Start()
    {
        MaxHealth = 100f;
        MaxExp = 100f;
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

    private void OnLevelUp(object playerLevelData)
    {
        int playerLevel = (int)playerLevelData;
        MaxHealth += playerLevel * 10;
        CurrentHealth = MaxHealth;
        MaxExp += playerLevel * 100;
        AttackDamage += playerLevel * 2;
        AttackSpeed += playerLevel * 0.1f;
        MovementSpeed += playerLevel * 0.2f;
    }

    private void OnPlayerDeath()
    {
        IsDead = true;
        playerAnimator.SetBool("dead", true);
        UnsubscribeEvents();
        PlayerUI.BlockInput();
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        if (restoreHealthCoroutine != null) StopCoroutine(restoreHealthCoroutine);
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SetUpEvents()
    {
        onPlayerDeath += OnPlayerDeath;
        EventManager.StartListening(UnityEventName.PlayerDeath, onPlayerDeath);
        onLevelUp += OnLevelUp;
        EventManager.StartListening(TypedEventName.LevelUp, onLevelUp);
        onPlayerDamage += OnPlayerDamage;
        EventManager.StartListening(TypedEventName.DamagePlayer, onPlayerDamage);
        onPlayerExperience += OnPlayerExperience;
        EventManager.StartListening(TypedEventName.AddExperience, onPlayerExperience);
    }

    private void UnsubscribeEvents()
    {
        EventManager.StopListening(UnityEventName.PlayerDeath, onPlayerDeath);
        EventManager.StopListening(TypedEventName.LevelUp, onLevelUp);
        EventManager.StopListening(TypedEventName.DamagePlayer, onPlayerDamage);
        EventManager.StopListening(TypedEventName.AddExperience, onPlayerExperience);
    }
}
