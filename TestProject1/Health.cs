using Xunit.Sdk;

/// <summary>
/// This class models the HP of an entity, health, shield, resistances, and vulnerabilities.
/// </summary>
public class Health
{
    /// <summary>
    /// Current hit points.
    /// </summary>
    public int _hp { get; private set; }

    /// <summary>
    /// Maximum hit points.
    /// </summary>
    public int _hpMax { get; private set; }

    /// <summary>
    /// Current shield points.
    /// </summary>
    public int _shield { get; private set; }

    /// <summary>
    /// Maximum shield points.
    /// </summary>
    public int _shieldMax { get; private set; }

    /// <summary>
    /// Resistance against physical damage.
    /// </summary>
    public int _resistancePhysical { get; private set; }

    /// <summary>
    /// Resistance against electric damage.
    /// </summary>
    public int _resistanceElectric { get; private set; }

    /// <summary>
    /// Vulnerability to physical damage.
    /// </summary>
    public int _vulnPhysical { get; private set; }

    /// <summary>
    /// Vulnerability to electric damage.
    /// </summary>
    public int _vulnElectric { get; private set; }

    /// <summary>
    /// Indicates whether the entity is alive.
    /// </summary>
    public bool _alive { get; private set; } = true;

    /// <summary>
    /// Damage bias applied to all damage calculations.
    /// </summary>
    public const int _damageBias = 12;

    /// <summary>
    /// Initializes a new instance of the Health class.
    /// </summary>
    /// <param name="hpMax">Maximum hit points.</param>
    /// <param name="shieldMax">Maximum shield points.</param>
    /// <param name="initialResistancePhysical">Initial physical resistance.</param>
    /// <param name="initialResistanceElectric">Initial electric resistance.</param>
    /// <param name="vulnPhysical">Physical vulnerability.</param>
    /// <param name="vulnElectric">Electric vulnerability.</param>
    public Health(int hpMax, int shieldMax, int initialResistancePhysical, int initialResistanceElectric, int vulnPhysical, int vulnElectric)
    {
        _hpMax = hpMax;
        _shieldMax = shieldMax;
        _hp = _hpMax;
        _shield = _hpMax;
        _resistancePhysical = initialResistancePhysical;
        _resistanceElectric = initialResistanceElectric;
        _vulnElectric = vulnElectric;
        _vulnPhysical = vulnPhysical;
    }

    /// <summary>
    /// Applies true damage to shield and HP, ignoring resistances.
    /// </summary>
    /// <param name="damage">The amount of true damage to apply.</param>
    public void ApplyTrueDamage(int damage)
    {
        int damageAfterShield = Math.Max(damage - _shield, 0);
        _shield = Math.Max(_shield - damage, 0);
        _hp = _hp - damageAfterShield;

        if (_hp <= 0)
        {
            _hp = 0;
            _alive = false;
        }
    }

    /// <summary>
    /// Applies physical and electric damage, taking into account resistances and vulnerabilities.
    /// </summary>
    /// <param name="damagePhysical">Physical damage amount.</param>
    /// <param name="damageElectric">Electric damage amount.</param>
    public void ApplyDamage(int damagePhysical, int damageElectric)
    {
        // line-by-line for easy debugging
        // doing stuff just twice doesn't warrant its own function

        int damagePhysicalAfterResistance = ((1000 - _resistancePhysical) * damagePhysical) / 1000;
        int damageElectricAfterResistance = ((1000 - _resistanceElectric) * damageElectric) / 1000;

        int damagePhysicalAfterVuln = ((1000 + _vulnPhysical) * damagePhysicalAfterResistance) / 1000;
        int damageElectricAfterVuln = ((1000 + _vulnElectric) * damageElectricAfterResistance) / 1000;

        int damageTotal = damagePhysicalAfterVuln + damageElectricAfterVuln;

        ApplyTrueDamage(damageTotal);
    }

    /// <summary>
    /// Heals a specified amount of HP.
    /// </summary>
    /// <param name="amount">Amount of HP to heal.</param>
    public void HealHP(int amount)
    {
        _hp = Math.Min(_hp + amount, _hpMax);
    }

    /// <summary>
    /// Heals a specified amount of shield.
    /// </summary>
    /// <param name="amount">Amount of shield to heal.</param>
    public void HealShield(int amount)
    {
        _shield = Math.Min(_shield + amount, _shieldMax);
    }

    /// <summary>
    /// Heals both HP and shield, prioritizing HP.
    /// </summary>
    /// <param name="healAmount">Total amount to heal.</param>
    public void HealBoth(int healAmount)
    {
        int healthMissing = _hpMax - _hp;

        _hp = Math.Min(_hp + healAmount, _hpMax);

        if (healAmount > healthMissing)
        {
            _shield = Math.Min(_shield + healAmount - healthMissing, _shieldMax);
        }
    }

    /// <summary>
    /// Completely destroys the shield.
    /// </summary>
    public void DestroyShield()
    {
        _shield = 0;
    }

    /// <summary>
    /// Restores HP to maximum.
    /// </summary>
    public void RestoreHealth()
    {
        _hp = _hpMax;
    }

    /// <summary>
    /// Restores shield to maximum.
    /// </summary>
    public void RestoreShield()
    {
        _shield = _shieldMax;
    }

    /// <summary>
    /// Damages the shield by a specified amount.
    /// </summary>
    /// <param name="damage">Amount of damage to apply to the shield.</param>
    public void damageShield(int damage)
    {
        _shield = Math.Max(_shield - damage, 0);
    }

    /// <summary>
    /// Damages HP by a specified amount.
    /// </summary>
    /// <param name="damage">Amount of damage to apply to HP.</param>
    public void damageHP(int damage)
    {
        _hp = Math.Max(_hp - damage, 0);
        _alive = Convert.ToBoolean(_hp);
    }

    /// <summary>
    /// Revives the entity with 1 HP and no shield.
    /// </summary>
    public void Revive()
    {
        _hp = 1;
        _shield = 0;
        _alive = true;
    }
}
