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
        Ready,
        Firing,
        Reloading
    }
    public enum MeleeState
    {
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
    public float bulletVelocity = 30f;
    public float BulletLifeTime = 4f;
    [Header("Weapon States and Type")]
    public PlayerState currentPlayerState;
    public GunState currentGunState;
    public MeleeState currentMeleeState;
    public WeaponType CurrentWeaponType;
    [Header("Animation Objects")]
    public GameObject GunObject;
    public GameObject MeleeObject;

    void Start()
    {
        BulletCount = Magazine;
        meleeDamage = PlayerDisplay.Instance.playerData.MeleeDamage;
        currentPlayerState = PlayerState.normal;
        CurrentWeaponType = WeaponType.Gun;
        currentGunState = GunState.Ready;
        currentMeleeState = MeleeState.Ready;
        InGameUIManager.Instance.UpdateAmmoText(BulletCount, Magazine);

        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
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
                SwitchToGunWeapon();
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (CurrentWeaponType == WeaponType.Gun && currentGunState == GunState.Ready)
            {
                SwitchToMeleeWeapon();
            }
        }
    }
    public void SwitchToGunWeapon()
    {
        CurrentWeaponType = WeaponType.Gun;
        GunObject.SetActive(true);
        MeleeObject.SetActive(false);
    }
    public void SwitchToMeleeWeapon()
    {
        CurrentWeaponType = WeaponType.Melee;
        GunObject.SetActive(false);
        MeleeObject.SetActive(true);
    }
    //input untuk weapon (fire, reload, melee attack)
    private void WeaponInput()
    {
        if(CurrentWeaponType == WeaponType.Gun)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && currentGunState == GunState.Ready && BulletCount > 0)
            {
                FireWeapon();
            }
            if(Input.GetKeyDown(KeyCode.R) && currentGunState != GunState.Reloading)
            {
                StartCoroutine(ReloadWeapon());
            }
        } else if(CurrentWeaponType == WeaponType.Melee)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && currentMeleeState == MeleeState.Ready)
            {
                //Implement melee attack
                StartCoroutine(MeleeAttack());
            }
        }
    }
    IEnumerator MeleeAttack()
    {
        currentMeleeState = MeleeState.Attacking;
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
        //instantiate peluru
        GameObject bullet = Instantiate(bulletPrefab, BulletSpawn.position, Quaternion.identity);
        //add force ke peluru
        bullet.GetComponent<Rigidbody>().AddForce(BulletSpawn.forward * bulletVelocity, ForceMode.Impulse);
        //destroy peluru setelah beberapa detik
        Destroy(bullet, BulletLifeTime);
        //kurangi jumlah peluru
        BulletCount--;
        InGameUIManager.Instance.UpdateAmmoText(BulletCount, Magazine);
    }
    IEnumerator ReloadWeapon()
    {
        InGameUIManager.Instance.ShowReloadText("Reloading...", true);
        currentGunState = GunState.Reloading;
        //wait for 2 seconds
        yield return new WaitForSeconds(1f);
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