using Xunit.Sdk;

public class Health
{
    public int _hp { get; private set; }
    public int _hpMax { get; private set; }
    public int _shield { get; private set; }
    public int _shieldMax { get; private set; }
    public int _resistancePhysical { get; private set; }
    public int _resistanceElectric { get; private set; }
    public int _vulnPhysical { get; private set; }
    public int _vulnElectric { get; private set; }
    public bool _alive { get; private set; } = true;
    public const int _damageBias = 12;

    public Health(int hpMax, int shieldMax, int initialResistancePhysical, int initialResistanceElectric, int vulnPhysical, int vulnElectric)
    {
        _hpMax = hpMax;
        _shieldMax = shieldMax;
        _hp = _hpMax;
        _shield = _hpMax;
        _resistancePhysical = initialResistancePhysical;
        _resistanceElectric = initialResistanceElectric;
        _vulnElectric = vulnElectric;
        _vulnElectric = vulnElectric;
    }

    public void ApplyTrueDamage(int damage)
    {
        int damageAfterShield = Math.Max(damage - _shield, 0);
        _shield = Math.Max(_shield - damage, 0);
        _hp = Math.Max(_hp - damageAfterShield, 0);

        if (_hp <= 0)
        {
            _hp = 0;
            _alive = false;
        }
    }
    public void ApplyDamage(int damagePhysical, int damageElectric)
    {
        // line-by-line for easy debugging
        // doing stuff just twice dowsn't warrant its own function

        int damagePhysicalAfterResistance = ((1000 - _resistancePhysical) * damagePhysical) / 1000;
        int damageElectricAfterResistance = ((1000 - _resistanceElectric) * damageElectric) / 1000;

        int damagePhysicalAfterVuln = ((1000 + _vulnPhysical) * damagePhysicalAfterResistance) / 1000;
        int damageElectricAfterVuln = ((1000 + _vulnElectric) * damageElectricAfterResistance) / 1000;

        int damageTotal = damagePhysicalAfterVuln + damageElectricAfterVuln;

        ApplyTrueDamage(damageTotal);
    }

    public void HealHP(int amount)
    {
        _hp = Math.Min(_hp + amount, _hpMax);
    }

    public void HealShield(int amount)
    {
        _shield = Math.Min(_shield + amount, _shieldMax);
    }

    public void HealBoth(int healAmount)
    {
        int healthMissing = _hpMax - _hp;
        _hp = Math.Min(_hp + healAmount, _hpMax);
        _shield = Math.Min(_shield + healAmount - healthMissing, _shieldMax);
    }

    public void DestroyShield()
    {
        _shield = 0;
    }
    public void RestoreHealth()
    {
        _hp = _hpMax;
    }

    public void RestoreShield()
    {
        _shield = _shieldMax;
    }
    public void Revive()
    {
        _hp = 1;
        _shield = 0;
        _alive = true;
    }
}
