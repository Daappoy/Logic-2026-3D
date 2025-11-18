using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;           // set by shooter
    public bool isPlayerBullet;  // true if from player, false if from enemy
    public float velocity = 30f; // bullet speed
    public float lifeTime = 4f; // bullet lifetime

    public void Initialize(int damage, bool isPlayerBullet)
    {
        this.damage = damage;
        this.isPlayerBullet = isPlayerBullet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayerBullet)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var enemy = collision.gameObject.GetComponent<EnemyDisplay>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
            return;
        }

        // enemy bullet hits player
        if (collision.gameObject.CompareTag("Player"))
        {
            //get the Gameobject's name
            Debug.Log(collision.gameObject.name);
            var player = collision.gameObject.GetComponent<PlayerDisplay>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
