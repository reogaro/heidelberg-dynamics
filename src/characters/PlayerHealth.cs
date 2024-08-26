using Godot;
using System;

/// <summary>
/// Represents the health of the player character.
/// </summary>
public partial class PlayerHealth : Node2D
{
	/// <summary>
	/// The player's health information.
	/// </summary>
	public Health health = new Health(1000, 1000, 100, 100, 100, 100);
}
