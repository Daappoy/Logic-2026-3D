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
            return;
        } 
        if (Time.time < nextHealTime) 
        {
            Debug.Log("Heal on Cooldown");
            return;
        }
        if (GameManager.Instance.currentState != GameManager.GameState.InGame) 
        {
            return;
        }

        StartCoroutine(HealOverTime());
        GameManager.Instance.HealUsed++;
    }
    public IEnumerator HealOverTime()
    {
        isHealing = true;
        nextHealTime = Time.time + Cooldown;
        int ticks = Duration / TickRate;
        for(int i = 0; i < ticks; i++)
        {
            if(PlayerDisplay.Instance.currentHealth < PlayerDisplay.Instance.MaxHealth)
            {
                PlayerDisplay.Instance.Heal(HealPoint);
            }
            yield return new WaitForSeconds(TickRate);
        }
        isHealing = false;
    }
    private void TryStartUltimate()
    {
        if (isUltimateActive)
        {
            return;
        } 
        if (Time.time < ultimateNextTime) 
        {
            Debug.Log("Ultimate on Cooldown");
            return; 
        }
        if (GameManager.Instance.currentState != GameManager.GameState.InGame) 
        {
            return;
        }

        StartCoroutine(UltimateAbility());
        GameManager.Instance.UltimateUsed++;
    }
    public IEnumerator UltimateAbility()
    {

        isUltimateActive = true;
        ultimateNextTime = Time.time + UltimateCooldown;
        Weapon.Instance.currentPlayerState = Weapon.PlayerState.ultimate;
        Weapon.Instance.StartCoroutine(Weapon.Instance.SwitchToMeleeWeapon());
        PlayerDisplay.Instance.SetMaxHealth(200);
        if(PlayerDisplay.Instance.currentHealth < 100)
        {
            int missing = PlayerDisplay.Instance.MaxHealth - PlayerDisplay.Instance.currentHealth;
            PlayerDisplay.Instance.Heal(missing);
        }

        yield return new WaitForSeconds(UltimateDuration);

        isUltimateActive = false;
        Weapon.Instance.currentPlayerState = Weapon.PlayerState.normal;
        PlayerDisplay.Instance.SetMaxHealth(100);
        if(Weapon.Instance.CurrentWeaponType == Weapon.WeaponType.Melee)
        {
            Weapon.Instance.currentGunState = Weapon.GunState.NotReady;
            Weapon.Instance.StartCoroutine(Weapon.Instance.SwitchToGunWeapon());
        }
    }
}
