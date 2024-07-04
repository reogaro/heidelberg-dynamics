using Godot;
using System;

public partial class explosion : AnimatedSprite2D
{
	public override void _Ready(){
		Position = new Vector2(0,0);
		//Position = GetParent().GlobalPosition;
	}

	private void _on_animation_finished(){
		QueueFree();
	}
}
