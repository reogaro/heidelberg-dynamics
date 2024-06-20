using Godot;
using System;

public partial class level_4 : Node2D
{
	public override void _Ready(){
		GetNode<Player>("Player").SetExtraCard(false);
		GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetValue("keycard2");
	}
}
