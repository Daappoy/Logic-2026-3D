using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance;
    public enum PlayerState
    {
        normal,
        ultimate
    }
    public enum WeaponType
    {
        Gun,
        Melee
    }
    public enum GunState
    {
        NotReady,
        Ready,
        Firing,
        Reloading
    }
    public enum MeleeState
    {
        NotReady,
        Ready,
        Attacking
    }
    [Header("Melee Settings")]
    public float meleeRange = 5f;
    public int meleeDamage;
    [Header("Magazine and Ammo")]
    public int Magazine = 5;
    public int BulletCount;
    [Header("Bullet Settings and References")]
    public GameObject bulletPrefab;
    public Transform BulletSpawn;
    [Header("Weapon States and Type")]
    public PlayerState currentPlayerState;
    public GunState currentGunState;
    public MeleeState currentMeleeState;
    public WeaponType CurrentWeaponType;
    [Header("Animation Objects")]
    public Animator gunAnimator;
    public Animator meleeAnimator;
    public GameObject GunObject;
    public GameObject MeleeObject;

    void Start()
    {
        BulletCount = Magazine;
        meleeDamage = PlayerDisplay.Instance.playerData.MeleeDamage;
        currentPlayerState = PlayerState.normal;
        CurrentWeaponType = WeaponType.Gun;
        currentGunState = GunState.NotReady;
        currentMeleeState = MeleeState.NotReady;
        InGameUIManager.Instance.UpdateAmmoText(BulletCount, Magazine);

        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        StartCoroutine(SwitchToGunWeapon());
    }

    void Update()
    {
        WeaponInput();
        SwitchWeapon();
        if(BulletCount == 0 && currentGunState != GunState.Reloading)
        {
            InGameUIManager.Instance.ShowReloadText("(R) Reload", true);
        }
    }
    //switch weapon
    public void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && currentPlayerState != PlayerState.ultimate)
        {
            if(CurrentWeaponType == WeaponType.Melee && currentMeleeState == MeleeState.Ready)
            {
                StartCoroutine(SwitchToGunWeapon());
            }
        }
        // if(Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     if (CurrentWeaponType == WeaponType.Gun && currentGunState == GunState.Ready)
        //     {
        //         SwitchToMeleeWeapon();
        //     }
        // }
    }
    public IEnumerator SwitchToGunWeapon()
    {
        if(!GameManager.Instance.LoseGameisTriggered)
        {
            currentMeleeState = MeleeState.NotReady;
            GunObject.SetActive(true);
            MeleeObject.SetActive(false);
            gunAnimator.SetTrigger("Equip");
            yield return new WaitForSeconds(2.2f);
            CurrentWeaponType = WeaponType.Gun;
            currentGunState = GunState.Ready;
        }
    }
    public IEnumerator SwitchToMeleeWeapon()
    {
        currentGunState = GunState.NotReady;
        MeleeObject.SetActive(true);
        GunObject.SetActive(false);
        meleeAnimator.SetTrigger("Equip");
        yield return new WaitForSeconds(0.25f);
        CurrentWeaponType = WeaponType.Melee;
        currentMeleeState = MeleeState.Ready;
    }
    //input untuk weapon (fire, reload, melee attack)
    private void WeaponInput()
    {
        if(CurrentWeaponType == WeaponType.Gun)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && currentGunState == GunState.Ready && BulletCount > 0 && GameManager.Instance.currentState == GameManager.GameState.InGame)
            {
                FireWeapon();
                AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.GunshotSFX);
            }
            if(Input.GetKeyDown(KeyCode.R) && currentGunState != GunState.Reloading)
            {
                StartCoroutine(ReloadWeapon());
            }
        } 
        else if(CurrentWeaponType == WeaponType.Melee)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && currentMeleeState == MeleeState.Ready && GameManager.Instance.currentState == GameManager.GameState.InGame)
            {
                //Implement melee attack
                StartCoroutine(MeleeAttack());
            }
        }
    }
    IEnumerator MeleeAttack()
    {
        currentMeleeState = MeleeState.Attacking;
        meleeAnimator.SetTrigger("Attack");
        //attack logic
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 2f, meleeRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyDisplay enemy = hit.GetComponent<EnemyDisplay>();
                if (enemy != null)
                {
                    enemy.TakeDamage(meleeDamage);
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        currentMeleeState = MeleeState.Ready;
    }
    void FireWeapon()
    {
        //animasi firing
        gunAnimator.SetTrigger("Shoot");
        //instantiate peluru
        GameObject bullet = Instantiate(bulletPrefab, BulletSpawn.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(PlayerDisplay.Instance.damage, true);
        //add force ke peluru
        bullet.GetComponent<Rigidbody>().AddForce(BulletSpawn.forward * bulletScript.velocity, ForceMode.Impulse);
        //destroy peluru setelah beberapa detik
        Destroy(bullet, bulletScript.lifeTime);
        //kurangi jumlah peluru
        BulletCount--;
        InGameUIManager.Instance.UpdateAmmoText(BulletCount, Magazine);
    }
    IEnumerator ReloadWeapon()
    {
        InGameUIManager.Instance.ShowReloadText("Reloading...", true);
        currentGunState = GunState.Reloading;
        gunAnimator.SetTrigger("Reload");
        yield return new WaitForSeconds(3.09f);
        BulletCount = Magazine;
        InGameUIManager.Instance.UpdateAmmoText(BulletCount, Magazine);
        InGameUIManager.Instance.ShowReloadText("", false);
        currentGunState = GunState.Ready;
    }

    //Debugging purpose: visualize melee range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 2f, meleeRange);
    }
}