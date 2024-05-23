using Godot;
using System;

public partial class Turret : CharacterBody2D
{
	private bool hasWeapon = true;
	private double firerate = 0.5;
	private bool seesPlayer=false;
	private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");
	private bool canFire = true;
	
	public override void _Ready(){
	}
	
	public override void _PhysicsProcess(double delta){
		seesPlayer = CheckLineOfSight();
		if(hasWeapon){
			LookAt(GetParent().GetNode<Player>("Player").GetPosition());
			if(seesPlayer && canFire){
				Fire();
			}
		}
	}
	
	public async void Fire(){
		Node bulletInstance = bullet.Instantiate();
		GetParent().AddChild(bulletInstance);
		canFire = false;
		await ToSignal(GetTree().CreateTimer(firerate), "timeout");
		canFire = true;
	}
	
	public bool CheckLineOfSight(){
		Vector2 playerPos = GetParent().GetNode<CharacterBody2D>("Player").Position;
		double distance = Mathf.Sqrt((playerPos.X - Position.X)*(playerPos.X - Position.X) + 
					(playerPos.Y - Position.Y) *(playerPos.Y - Position.Y));
		if(distance < 200){
				return true;
		}
		return false;
	}
}
