using Godot;
using System;

/// <summary>
/// This class represents a turret in a 2D game.
/// </summary>
public partial class Turret : CharacterBody2D
{
    /// <summary>
    /// Indicates whether the turret has a weapon.
    /// </summary>
    private bool hasWeapon = true;

    /// <summary>
    /// The turret's firing rate in seconds.
    /// </summary>
    private double firerate = 0.65;

    /// <summary>
    /// Indicates whether the turret can see the player.
    /// </summary>
    private bool seesPlayer = false;

    /// <summary>
    /// The packed scene for the bullet.
    /// </summary>
    private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");

    /// <summary>
    /// Indicates whether the turret can fire.
    /// </summary>
    private bool canFire = true;

    /// <summary>
    /// Initializes the turret.
    /// </summary>
    public override void _Ready()
    {
    }

    /// <summary>
    /// Godot update function. Gets called every frame to update the turret's logic.
    /// </summary>
    /// <param name="delta">The time delta in seconds.</param>
    public override void _PhysicsProcess(double delta)
    {
        if (!GetNode<TurretHealth>("Health").health._alive)
        {
            hasWeapon = false;
            GetNode<Sprite2D>("Turret").Visible = false;
        }

        seesPlayer = CheckLineOfSight();

        if (hasWeapon)
        {
            LookAt(GetParent().GetNode<Player>("Player").GetPosition());

            if (seesPlayer && canFire)
            {
                Fire();
            }
        }
    }

    /// <summary>
    /// Fires the turret's weapon.
    /// </summary>
    public async void Fire()
    {
        GetNode<AudioStreamPlayer>("BulletSound").Play();

        RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
        GetParent().AddChild(bulletInstance);
        bulletInstance.Position = GetNode<Node2D>("BulletPoint").GlobalPosition;
        bulletInstance.RotationDegrees = RotationDegrees;
        bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation), new Vector2());

        canFire = false;
        await ToSignal(GetTree().CreateTimer(firerate), "timeout");
        canFire = true;
    }

    /// <summary>
    /// Checks if the turret can see the player.
    /// </summary>
    /// <returns>True if the turret can see the player, otherwise false.</returns>
    public bool CheckLineOfSight()
    {
        Vector2 playerPos = GetParent().GetNode<CharacterBody2D>("Player").Position;
        double distance = Mathf.Sqrt((playerPos.X - Position.X) * (playerPos.X - Position.X) +
                                    (playerPos.Y - Position.Y) * (playerPos.Y - Position.Y));

        if (distance < 300)
        {
            return true;
        }

        return false;
    }
}
