using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stat")]
    public Stat strength;
    public Stat agility;
    public Stat intellignece;
    public Stat vitality;

    [Header("Attack Stat")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Defense Stat")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stat")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    [SerializeField] private int currentHealth;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }


    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intellignece.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return; // if magic damage is <= 0, ailments are not applied
        }

        // Determine if ignite, chill, or shock ailments can be applied based on the highest damage type
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        int maxAttempts = 10; // Limit the number of attempts to avoid infinite loops
        int attempts = 0;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock && attempts < maxAttempts)
        {
            attempts++;

            // Randomly attempt to apply ignite if fire damage is greater than 0
            if (Random.value < 0.35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                break;
            }

            // Randomly attempt to apply chill if ice damage is greater than 0
            if (Random.value < 0.25f && _iceDamage > 0)
            {
                canApplyChill = true;
                break;
            }

            // Randomly attempt to apply shock if lightning damage is greater than 0
            if (Random.value < 0.15f && _lightningDamage > 0)
            {
                canApplyShock = true;
                break;
            }
        }

        // Apply the determined ailments (ignite, chill, or shock) to the target
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

        if (canApplyIgnite) Debug.Log("Ignite applied");
        if (canApplyChill) Debug.Log("Chill applied");
        if (canApplyShock) Debug.Log("Shock applied");
    }


    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intellignece.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }


    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }


    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }


    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
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


    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }


    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        // Debug.Log("total crit power %" + totalCritPower);

        float critDamage = _damage * totalCritPower;
        // Debug.Log("crit damage before round up" + critDamage);

        return Mathf.RoundToInt(critDamage);
    }
}
