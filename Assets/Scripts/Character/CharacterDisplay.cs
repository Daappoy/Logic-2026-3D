using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    public int MaxHealth;
    public int currentHealth;
    public int damage;

    public event System.Action<int,int> OnHealthChanged; // (current, max)

    protected void RaiseHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
    }
}
