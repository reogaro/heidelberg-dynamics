using Godot;
using System;

public partial class projectile : RigidBody2D
{
	private PackedScene explosion = GD.Load<PackedScene>("res://projectiles/explosion.tscn");
	private Timer timer;
	
	public override void _Ready(){
		timer = new Timer();
		this.AddChild(timer);
		timer.WaitTime = 0.34;
		timer.OneShot = true;
		timer.Start();
	}

	private void _on_body_entered(Node body){
		if(!body.IsInGroup("entity")){
			QueueFree();
		}
		else if(body.IsInGroup("player")){
			QueueFree();
			body.GetNode<PlayerHealth>("Health").health.ApplyDamage(50,50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
		else if(body.IsInGroup("boss")){
			QueueFree();
			body.GetNode<BossHealth>("Health").health.ApplyDamage(50,50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
		else{
			QueueFree();
			body.GetNode<TurretHealth>("Health").health.ApplyDamage(50,50);
			body.GetNode<AudioStreamPlayer>("HitSound").Play();
		}
	}

	public override void _PhysicsProcess(double delta){
		if (timer.IsStopped())
			QueueFree();
		}
	
	
}
