using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyDisplay>().TakeDamage(PlayerDisplay.Instance.GunDamage);
            Destroy(this.gameObject);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }
}
