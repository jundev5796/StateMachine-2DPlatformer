using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int damage;
    public int maxHp;

    [SerializeField] private int currentHealth;


    void Start()
    {
        currentHealth = maxHp;    
    }


    public void TakeDamage(int _damage)
    {
        currentHealth -= damage;
    }
}
