using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "ScriptableObjects/Player")]
public class Player : ScriptableObject
{
    public int health;
    public int GunDamage;
    public int MeleeDamage;
}
