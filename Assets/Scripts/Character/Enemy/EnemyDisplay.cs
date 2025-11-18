using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : CharacterDisplay
{
    public event System.Action OnEnemyDie;
    public AudioSource enemyAudioSource;
    public EnemyAI enemyAI;
    public Slider EnemyHealthbar;
    public Enemy enemyData;
    private void Awake()
    {
        if(enemyData != null)
        {
            MaxHealth = enemyData.Health;
            currentHealth = enemyData.Health;
            damage = enemyData.damage;
            RaiseHealthChanged();
        }
        OnHealthChanged += HandleHealthChanged;
    }
    private void OnDestroy()
    {
        OnHealthChanged -= HandleHealthChanged;
    }
    
    [ContextMenu("Take Damage")]
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Enemy takes " + damageAmount + " damage.");
        currentHealth -= damageAmount;
        RaiseHealthChanged();
        if(currentHealth <= 0)
        {
            GameManager.Instance.ScoreIncrease();
            GameManager.Instance.EnemyKilled += 1;
            StartCoroutine(DestroySelfAfterDelay());
            enemyAI.Die();
            Die();
        }
    }
    public IEnumerator DestroySelfAfterDelay()
    {
        enemyAudioSource.PlayOneShot(AudioManager.AudioInstance.DeathSFX);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void UpdateHealthBar(int current, int max)
    {
        EnemyHealthbar.maxValue = max;
        EnemyHealthbar.value = current;
    }
    private void HandleHealthChanged(int current, int max)
    {
        UpdateHealthBar(current, max);
    }
    private void Die()
    {
        OnEnemyDie?.Invoke();
    }
}
