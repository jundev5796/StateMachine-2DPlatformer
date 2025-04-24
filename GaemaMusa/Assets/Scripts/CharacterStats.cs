using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth.GetValue();

        // Equip sword +4
        damage.AddModifier(4);
    }


    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
            Die();
    }


    private void Die()
    {

    }
}
