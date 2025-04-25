using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stat")]
    public Stat strength;
    public Stat agility;
    public Stat intellignece;
    public Stat vitality;

    [Header("Defense Stat")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    public Stat damage;
    
    [SerializeField] private int currentHealth;


    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (CanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }


    private bool CanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }


    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
            Die();
    }


    protected virtual void Die()
    {

    }
}
