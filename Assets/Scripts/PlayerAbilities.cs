using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Ability E")]
    public int HealPoint = 10;
    public int Duration = 5;
    public int TickRate = 1;
    public int Cooldown = 10;
    private bool isHealing = false;
    private float nextHealTime = 0f;
    [Header("Ability Q - Ultimate")]
    public int UltimateDuration = 20;
    public int UltimateCooldown = 60;
    private float ultimateNextTime = 0f;
    private bool isUltimateActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartHeal();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            TryStartUltimate();
        }
    }

    private void TryStartHeal()
    {
        if (isHealing)
        {
            return; // already running
        } 
        if (Time.time < nextHealTime) 
        {
            Debug.Log("Heal on Cooldown");
            return; // cooldown not finished
        }
        if (GameManager.currentState != GameManager.GameState.InGame) 
        {
            return; //kalo lagi di pause
        }

        StartCoroutine(HealOverTime());
    }
    public IEnumerator HealOverTime()
    {
        isHealing = true;
        nextHealTime = Time.time + Cooldown;
        int ticks = Duration / TickRate;
        for(int i = 0; i < ticks; i++)
        {
            PlayerDisplay.Instance.currentHealth += HealPoint;
            InGameUIManager.Instance.UpdateHealthBar();
            yield return new WaitForSeconds(TickRate);
        }
        isHealing = false;
    }
    private void TryStartUltimate()
    {
        if (isUltimateActive)
        {
            return; // already running
        } 
        if (Time.time < ultimateNextTime) 
        {
            Debug.Log("Ultimate on Cooldown");
            return; // cooldown not finished
        }
        if (GameManager.currentState != GameManager.GameState.InGame) 
        {
            return; //kalo lagi di pause
        }

        StartCoroutine(UltimateAbility());
    }
    public IEnumerator UltimateAbility()
    {
        isUltimateActive = true;
        ultimateNextTime = Time.time + UltimateCooldown;
        Weapon.Instance.currentPlayerState = Weapon.PlayerState.ultimate;
        Weapon.Instance.SwitchToMeleeWeapon();
        PlayerDisplay.Instance.MaxHealth = 200;
        if(PlayerDisplay.Instance.currentHealth < 100)
        {
            PlayerDisplay.Instance.currentHealth = 200;
        }
        InGameUIManager.Instance.UpdateHealthBar();

        yield return new WaitForSeconds(UltimateDuration);

        isUltimateActive = false;
        Weapon.Instance.currentPlayerState = Weapon.PlayerState.normal;
        PlayerDisplay.Instance.MaxHealth = 100;
        if(PlayerDisplay.Instance.currentHealth > 100)
        {
            PlayerDisplay.Instance.currentHealth = 100;
        }
        InGameUIManager.Instance.UpdateHealthBar();
    }
}
