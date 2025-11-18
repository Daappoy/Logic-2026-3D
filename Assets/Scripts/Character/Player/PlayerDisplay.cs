using UnityEngine;

public class PlayerDisplay : CharacterDisplay
{
    public static PlayerDisplay Instance;
    public Weapon weaponScript;
    public Player playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if(playerData != null)
        {
            currentHealth = playerData.health;
            damage = playerData.GunDamage;
            MaxHealth = playerData.health;
            RaiseHealthChanged();
        }
    }
    void Start()
    {
        RaiseHealthChanged();
    }

    [ContextMenu("Take Damage")] 
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Player takes " + damageAmount + " damage.");
        currentHealth -= damageAmount;
        RaiseHealthChanged();
        if(currentHealth <= 0 && !GameManager.Instance.LoseGameisTriggered)
        {
            AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.DeathSFX);
            GameManager.Instance.LoseGame();
            weaponScript.GunObject.SetActive(false);
            weaponScript.MeleeObject.SetActive(false);
        }
    }
    void Update()
    {
        if(transform.position.y < -10)
        {
            GameManager.Instance.LoseGame();
            weaponScript.GunObject.SetActive(false);
            weaponScript.MeleeObject.SetActive(false);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > MaxHealth) currentHealth = MaxHealth;
        RaiseHealthChanged();
    }

    public void SetMaxHealth(int newMax)
    {
        MaxHealth = newMax;
        if(currentHealth > MaxHealth) currentHealth = MaxHealth;
        RaiseHealthChanged();
    }
}