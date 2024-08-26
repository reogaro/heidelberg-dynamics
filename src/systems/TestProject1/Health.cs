using System;

/// <summary>
/// Represents the health and other attributes of a character.
/// </summary>
public class Health
{
	/// <summary>
	/// The current health points.
	/// </summary>
	public int _hp { get; private set; }

	/// <summary>
	/// The maximum health points.
	/// </summary>
	public int _hpMax { get; private set; }

	/// <summary>
	/// The current shield points.
	/// </summary>
	public int _shield { get; private set; }

	/// <summary>
	/// The maximum shield points.
	/// </summary>
	public int _shieldMax { get; private set; }

	/// <summary>
	/// The physical resistance.
	/// </summary>
	public int _resistancePhysical { get; private set; }

	/// <summary>
	/// The electric resistance.
	/// </summary>
	public int _resistanceElectric { get; private set; }

	/// <summary>
	/// The vulnerability to physical damage.
	/// </summary>
	public int _vulnPhysical { get; private set; }

	/// <summary>
	/// The vulnerability to electric damage.
	/// </summary>
	public int _vulnElectric { get; private set; }

	/// <summary>
	/// Indicates if the character is alive.
	/// </summary>
	public bool _alive { get; private set; } = true;

	/// <summary>
	/// A constant representing the damage bias.
	/// </summary>
	public const int _damageBias = 12;

	/// <summary>
	/// Initializes a new Health instance.
	/// </summary>
	/// <param name="hpMax">The maximum health points.</param>
	/// <param name="shieldMax">The maximum shield points.</param>
	/// <param name="initialResistancePhysical">The initial physical resistance.</param>
	/// <param name="initialResistanceElectric">The initial electric resistance.</param>
	/// <param name="vulnPhysical">The vulnerability to physical damage.</param>
	/// <param name="vulnElectric">The vulnerability to electric damage.</param>
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
	/// Applies true damage to the character, bypassing resistances and vulnerabilities.
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
	/// Applies damage to the character based on physical and electric damage values.
	/// </summary>
	/// <param name="damagePhysical">The physical damage.</param>
	/// <param name="damageElectric">The electric damage.</param>
	public void ApplyDamage(int damagePhysical, int damageElectric)
	{
		// Calculate damage after resistances and vulnerabilities
		int damagePhysicalAfterResistance = ((1000 - _resistancePhysical) * damagePhysical) / 1000;
		int damageElectricAfterResistance = ((1000 - _resistanceElectric) * damageElectric) / 1000;

		int damagePhysicalAfterVuln = ((1000 + _vulnPhysical) * damagePhysicalAfterResistance) / 1000;
		int damageElectricAfterVuln = ((1000 + _vulnElectric) * damageElectricAfterResistance) / 1000;

		int damageTotal = damagePhysicalAfterVuln + damageElectricAfterVuln;

		ApplyTrueDamage(damageTotal);
	}

	/// <summary>
	/// Heals the character's health.
	/// </summary>
	/// <param name="amount">The amount of health to heal.</param>
	public void HealHP(int amount)
	{
		_hp = Math.Min(_hp + amount, _hpMax);
	}

	/// <summary>
	/// Heals the character's shield.
	/// </summary>
	/// <param name="amount">The amount of shield to heal.</param>
	public void HealShield(int amount)
	{
		_shield = Math.Min(_shield + amount, _shieldMax);
	}

	/// <summary>
	/// Heals both the character's health and shield.
	/// </summary>
	/// <param name="healAmount">The amount to heal.</param>
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
	/// Destroys the character's shield.
	/// </summary>
	public void DestroyShield()
	{
		_shield = 0;
	}

	/// <summary>
	/// Restores the character's health to full.
	/// </summary>
	public void RestoreHealth()
	{
		_hp = _hpMax;
	}

	/// <summary>
	/// Restores the character's shield to full.
	/// </summary>
	public void RestoreShield()
	{
		_shield = _shieldMax;
	}

	/// <summary>
	/// Damages the character's shield.
	/// </summary>
	/// <param name="damage">The amount of damage to apply to the shield.</param>
	public void DamageShield(int damage)
	{
		_shield = Math.Max(_shield - damage, 0);
	}

	/// <summary>
	/// Damages the character's health.
	/// </summary>
	/// <param name="damage">The amount of damage to apply to the health.</param>
	public void DamageHP(int damage)
	{
		_hp = Math.Max(_hp - damage, 0);
		_alive = Convert.ToBoolean(_hp);
	}

	/// <summary>
	/// Revives the character.
	/// </summary>
	public void Revive()
	{
		_hp = 1;
		_shield = 0;
		_alive = true;
	}
}
