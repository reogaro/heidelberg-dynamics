using Godot;
using System;

public partial class projectile : RigidBody2D
{
	private float speed = 750;
	private PackedScene explosion = GD.Load<PackedScene>("res://projectiles/explosion.tscn");
	
	public override void _Ready(){
		Position = GetParent().GetNode<CharacterBody2D>("Player").GetNode<Node2D>("BulletPoint").GlobalPosition;
		RotationDegrees = GetParent().GetNode<CharacterBody2D>("Player").RotationDegrees;
		ApplyImpulse(new Vector2(speed, 0).Rotated(GetParent().GetNode<CharacterBody2D>("Player").Rotation),new Vector2());
	}

	private void _on_body_entered(Node body){
		if(!body.IsInGroup("entitiy")){
			Node explosionInstance = explosion.Instantiate();
			AddChild(explosionInstance);
			QueueFree();
		}
	}
}
