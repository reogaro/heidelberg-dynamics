using Godot;
using System;

/// <summary>
/// Represents the health of a boss turret.
/// </summary>
public partial class BossHealth : Node2D
{
	/// <summary>
	/// The boss's health information.
	/// </summary>
	public Health health = new Health(1000, 1000, 100, 100, 100, 100);
}
