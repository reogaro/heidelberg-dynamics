using Godot;
using System;

/// <summary>
/// Represents the health of a turret in the game.
/// </summary>
public partial class TurretHealth : Node2D
{
	/// <summary>
	/// The health object associated with the turret.
	/// </summary>
	public Health health = new Health(100,100,100,100,100,100);
}
