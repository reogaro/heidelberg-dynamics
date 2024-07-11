using Godot;
using System;

public partial class Bossturret : CharacterBody2D
{
	private bool hasWeapon = true;
	private bool counter = false;
	private double firerate = 0.7;
	private bool seesPlayer=false;
	private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");
	private bool canFire = true;
	
	public override void _PhysicsProcess(double delta){
		if(!GetNode<BossHealth>("Health").health._alive){
			hasWeapon = false;
			if(!counter){
				GetParent().GetNode<Player>("Player").IncreaseBossCount();
				counter = true;
			}
			GetNode<Sprite2D>("Turret").Visible = false;
		}
		seesPlayer = CheckLineOfSight();
		if(hasWeapon){
			LookAt(GetParent().GetNode<Player>("Player").GetPosition());
			if(seesPlayer && canFire){
				Fire1();
				Fire2();
			}
		}
	}
	
	public async void Fire1(){
		GetNode<AudioStreamPlayer>("BulletSound").Play();
		RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
		GetParent().AddChild(bulletInstance);
		bulletInstance.Position = GetNode<Node2D>("BulletPoint1").GlobalPosition;
		bulletInstance.RotationDegrees = RotationDegrees;
		bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation),new Vector2());
		canFire = false;
		await ToSignal(GetTree().CreateTimer(firerate), "timeout");
		canFire = true;
	}
	public async void Fire2(){
		await ToSignal(GetTree().CreateTimer(0.35), "timeout");
		GetNode<AudioStreamPlayer>("BulletSound").Play();
		RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
		GetParent().AddChild(bulletInstance);
		bulletInstance.Position = GetNode<Node2D>("BulletPoint2").GlobalPosition;
		bulletInstance.RotationDegrees = RotationDegrees;
		bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation),new Vector2());
	}
	
	public bool CheckLineOfSight(){
		Vector2 playerPos = GetParent().GetNode<CharacterBody2D>("Player").Position;
		double distance = Mathf.Sqrt((playerPos.X - Position.X)*(playerPos.X - Position.X) + 
					(playerPos.Y - Position.Y) *(playerPos.Y - Position.Y));
		if(distance < 300){
				return true;
		}
		return false;
	}
}
