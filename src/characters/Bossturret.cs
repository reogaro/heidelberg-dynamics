using Godot;
using System;

/// <summary>
/// Represents a boss turret
/// </summary>
public partial class Bossturret : CharacterBody2D
{
    /// <summary>
    /// Indicates whether the turret has a weapon.
    /// </summary>
    private bool hasWeapon = true;

    /// <summary>
    /// Counts down the delay between bullets for double barrel altenate fire.
    /// </summary>
    private bool counter = false;

    /// <summary>
    /// The turret's firing rate in seconds.
    /// </summary>
    private double firerate = 0.7;

    /// <summary>
    /// Indicates whether the turret can see the player.
    /// </summary>
    private bool seesPlayer = false;

    /// <summary>
    /// The packed scene that displays the bullet.
    /// </summary>
    private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");

    /// <summary>
    /// Indicates whether the turret can fire.
    /// </summary>
    private bool canFire = true;


    /// <summary>
    /// Godot update function. Gets called every frame to update the turret's logic.
    /// </summary>
	public override void _PhysicsProcess(double delta){
		if(!GetNode<BossHealth>("Health").health._alive){
            hasWeapon = false;
            if (!counter)
            {
                GetParent().GetNode<Player>("Player").IncreaseBossCount();
                counter = true;
            }
            GetNode<Sprite2D>("Turret").Visible = false;
        }

        seesPlayer = CheckLineOfSight();
        if (hasWeapon)
        {
            LookAt(GetParent().GetNode<Player>("Player").GetPosition());
            if (seesPlayer && canFire)
            {
                Fire1();
                Fire2();
            }
        }
    }

    /// <summary>
    /// Fires the first bullet of the turret's weapon.
    /// </summary>
    public async void Fire1()
    {
        GetNode<AudioStreamPlayer>("BulletSound").Play();
        RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
        GetParent().AddChild(bulletInstance);
        bulletInstance.Position = GetNode<Node2D>("BulletPoint1").GlobalPosition;
        bulletInstance.RotationDegrees = RotationDegrees;
        bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation), new Vector2());
        canFire = false;
        await ToSignal(GetTree().CreateTimer(firerate), "timeout");
        canFire = true;
    }

    /// <summary>
    /// Fires the second bullet of the turret's weapon.
    /// </summary>
    public async void Fire2()
    {
        await ToSignal(GetTree().CreateTimer(0.35), "timeout");
        GetNode<AudioStreamPlayer>("BulletSound").Play();
        RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
        GetParent().AddChild(bulletInstance);
        bulletInstance.Position = GetNode<Node2D>("BulletPoint2").GlobalPosition;
        bulletInstance.RotationDegrees = RotationDegrees;
        bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation), new Vector2());
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
