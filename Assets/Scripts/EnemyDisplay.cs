using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 velocity;
    public float gravity = -9.81f;
    public Slider EnemyHealthbar;
    public Enemy enemyData;
    public int currentHealth;
    public int damage;

    private void Awake()
    {
        if(enemyData != null)
        {
            currentHealth = enemyData.Health;
            damage = enemyData.damage;
        }
    }
    private void Update()
    {
        Gravity();
    }
    [ContextMenu("Take Damage")]
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();
        if(currentHealth <= 0)
        {
            DestroySelf();
            GameManager.Instance.EnemyKilled += 1;
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        EnemyHealthbar.value = currentHealth;
    }
    
    //enemy gravity
    private void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
