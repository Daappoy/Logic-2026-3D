using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    public static PlayerDisplay Instance;
    public Player playerData;
    public int MaxHealth;
    public int currentHealth;
    public int GunDamage;

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
            GunDamage = playerData.GunDamage;
            MaxHealth = playerData.health;
        }
    }
    void Start()
    {
        InGameUIManager.Instance.UpdateHealthBar();
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        currentHealth -= GunDamage;
        InGameUIManager.Instance.UpdateHealthBar();
        if(currentHealth <= 0)
        {
            DestroySelf();
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}