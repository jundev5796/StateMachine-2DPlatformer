using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public Stat strength;     // Strength - Increases physical damage
    public Stat agility;      // Agility - Increases evasion and critical hit chance
    public Stat intelligence; // Intelligence - Increases magical damage
    public Stat vitality;     // Vitality - Increases maximum health

    [Header("Attack Stats")]
    public Stat damage;       // Base damage
    public Stat critChance;   // Critical hit chance
    public Stat critPower;    // Critical hit damage multiplier

    [Header("Defense Stats")]
    public Stat maxHealth;    // Maximum health
    public Stat armor;        // Physical defense
    public Stat evasion;      // Evasion rate
    public Stat magicResistance; // Magic resistance

    [Header("Magic Stats")]
    public Stat fireDamage;    // Fire damage
    public Stat iceDamage;     // Ice damage
    public Stat lightingDamage; // Lightning damage

    public bool isIgnited;    // Whether the character is ignited
    public bool isChilled;    // Whether the character is chilled
    public bool isShocked;    // Whether the character is shocked

    private float ignitedTimer; // Duration of ignite status
    private float chilledTimer; // Duration of chill status
    private float shockedTimer; // Duration of shock status

    private float igniteDamageCooldown = 0.3f; // Ignite damage cooldown
    private float igniteDamageTimer; // Ignite damage timer

    private int igniteDamage; // Ignite damage

    public int currentHealth; // Current health

    public System.Action onHealthChanged; // Handle damage effect (override in child classes)


    protected virtual void Start()
    {
        // Set default critical hit damage multiplier (150%)
        critPower.SetDefaultValue(150);
        // Initialize current health to maximum health
        currentHealth = GetMaxHealthValue();
    }


    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; // Decrease ignite status duration
        chilledTimer -= Time.deltaTime; // Decrease chill status duration
        shockedTimer -= Time.deltaTime; // Decrease shock status duration

        igniteDamageTimer -= Time.deltaTime; // Decrease ignite damage cooldown
        if (ignitedTimer < 0)
            isIgnited = false; // Remove ignite status

        if (chilledTimer < 0)
            isChilled = false; // Remove chill status

        if (shockedTimer < 0)
            isShocked = false; // Remove shock status

        if (igniteDamageTimer < 0 && isIgnited) // If ignite damage cooldown is over and character is ignited
        {
            Debug.Log("Ignite damage applied: " + igniteDamage);
            DecreaseHealth(igniteDamage); // Subtract ignite damage from current health

            if (currentHealth < 0)
                Die(); // If health is 0 or below, handle death

            igniteDamageTimer = igniteDamageCooldown; // Reset cooldown
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // Check if the target can avoid the attack
        if (TargetCanAvoidAttack(_targetStats))
            return;

        // Calculate physical damage (base damage + strength)
        int totalDamage = damage.GetValue() + strength.GetValue();

        // Check for critical hit and calculate damage
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        // Apply target's armor
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        // Apply damage
        _targetStats.TakeDamage(totalDamage);
        // Apply magical damage
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        // Calculate damage for each magic type
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        // Calculate total magical damage (each type of damage + intelligence)
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        // Apply target's magic resistance
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        // Apply damage
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return; // Do not apply status effects if all damage types are 0 or below
        }

        // Check conditions for applying status effects
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;  // Apply ignite if fire damage is highest
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;     // Apply chill if ice damage is highest
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage; // Apply shock if lightning damage is highest

        // Loop if no status effect conditions are met
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            // 35% chance to apply ignite if fire damage is greater than 0
            if (Random.value < 0.35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Apply status effects
                Debug.Log("Ignite status applied");
                return;
            }

            // 25% chance to apply chill if ice damage is greater than 0
            if (Random.value < 0.25f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Apply status effects
                Debug.Log("Chill status applied");
                return;
            }

            // 15% chance to apply shock if lightning damage is greater than 0
            if (Random.value < 0.15f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Apply status effects
                Debug.Log("Shock status applied");
                return;
            }

            // Loop continues if none of the conditions are met
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f)); // Set ignite damage

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // Apply status effects
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        // Subtract target's magic resistance and intelligence bonus from magical damage
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        // Clamp to ensure damage does not go below 0
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // Ignore if any status effect is already applied
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;  // Apply ignite status
            ignitedTimer = 2; // Set ignite status duration
        }

        if (_chill)
        {
            isChilled = _chill; // Apply chill status
            chilledTimer = 2; // Set chill status duration
        }
        if (_shock)
        {
            isShocked = _shock; // Apply shock status
            shockedTimer = 2; // Set shock status duration
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage; // Set ignite damage

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f); // Reduce armor by 20% if chilled
        else
            totalDamage -= _targetStats.armor.GetValue(); // Subtract armor

        // Clamp to ensure damage does not go below 0
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        // Calculate total evasion (evasion + agility)
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20; // Increase evasion by 20% if shocked
        }

        // Check evasion chance (if random value between 0 and 100 is less than total evasion, attack is avoided)
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        // Subtract damage from current health
        DecreaseHealth(_damage);

        Debug.Log(_damage);

        // Handle death if health is 0 or below
        if (currentHealth < 0)
            Die();
        
        onHealthChanged?.Invoke(); // Invoke health changed event
    }


    protected virtual void DecreaseHealth(int _damage)
  {
     currentHealth -= _damage; // 현재 체력에서 데미지 차감
      
      if(onHealthChanged != null)
        onHealthChanged(); // 체력 변경 시 델리게이트 호출
  }


    protected virtual void Die()
    {
        // Override in child classes to implement death handling
    }

    private bool CanCrit()
    {
        // Calculate total critical hit chance (critical hit chance + agility)
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        // Check critical hit chance (if random value between 0 and 100 is less than or equal to total critical chance, critical hit occurs)
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        // Calculate total critical hit multiplier ((critical hit power + strength) / 100)
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        // Calculate critical hit damage (base damage * critical hit multiplier)
        float critDamage = _damage * totalCritPower;

        // Round to nearest integer and return
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5; // Calculate maximum health (base maximum health + vitality * 5)
    }
}
