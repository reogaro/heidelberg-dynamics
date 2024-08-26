using Godot;
using System;

/// <summary>
/// Represents the logic for level 4 of the game.
/// </summary>
public partial class Level_4 : Node2D
{
	/// <summary>
	/// Initializes the level.
	/// </summary>
	public override void _Ready()
	{
		// Reset the player's extra card state to false
		GetNode<Player>("Player").SetExtraCard(false);

		// Set the interaction value of the keycard2 to "keycard2"
		GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetValue("keycard2");
	}
}
