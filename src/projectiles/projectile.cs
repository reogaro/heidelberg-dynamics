using Godot;
using System;

/// <summary>
/// Represents a projectile entity in the game.
/// </summary>
public partial class Projectile : RigidBody2D
{
	/// <summary>
	/// PackedScene reference to the explosion effect.
	/// </summary>
	private PackedScene explosion = GD.Load<PackedScene>("res://projectiles/explosion.tscn");

	/// <summary>
	/// Timer to manage the projectile's lifespan.
	/// </summary>
	private Timer timer;

	/// <summary>
	/// Initializes the projectile.
	/// </summary>
	public override void _Ready()
	{
		// Create a timer to destroy the projectile after a set duration.
		timer = new Timer();
		this.AddChild(timer);
		timer.WaitTime = 0.34; // Adjust the lifespan as needed
		timer.OneShot = true;
		timer.Start();
	}

	/// <summary>
	/// Handles collisions with other game objects.
	/// </summary>
	/// <param name="body">The body that the projectile collided with.</param>
	private void _on_body_entered(Node body)
	{
		// Check if the collision is with a valid target
		if (!body.IsInGroup("entity"))
		{
			// If not, destroy the projectile
			QueueFree();
		}
		else if (body.IsInGroup("player"))
		{
			// If it's the player, damage them and play a hit sound
			QueueFree();
			body.GetNode<PlayerHealth>("Health").health.ApplyDamage(50, 50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
		else if (body.IsInGroup("boss"))
		{
			// If it's a boss, damage them and play a hit sound
			QueueFree();
			body.GetNode<BossHealth>("Health").health.ApplyDamage(50, 50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
		else
		{
			// If it's a turret, damage it and play a hit sound
			QueueFree();
			body.GetNode<TurretHealth>("Health").health.ApplyDamage(50, 50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
	}

	/// <summary>
	/// Updates the projectile's state.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _PhysicsProcess(double delta)
	{
		// Destroy the projectile if the timer has expired
		if (timer.IsStopped())
		{
			QueueFree();
		}
	}
}
