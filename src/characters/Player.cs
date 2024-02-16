using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private float moveSpeed = 250;
	private bool gotKeycard = false;
	private InteractionArea currentInteract;
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback stateMachine;
	private double firerate = 0.5;
	private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");
	private bool canFire = true;
	
	public override void _Ready(){
		this.gotKeycard = false;
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}
	
	public override void _PhysicsProcess(double delta){
		LookAt(GetGlobalMousePosition());
		//get input directions
		Vector2 inputDirection = new Vector2(
		Input.GetActionRawStrength("right") - Input.GetActionRawStrength("left"),
		Input.GetActionRawStrength("down") - Input.GetActionRawStrength("up")
		);
		//fire bullet if pressed
		if(Input.IsActionPressed("fire")&& canFire){
			Fire();
		}
		//execute interaction if needed
		if(Input.IsActionPressed("interact")){
			ExecuteInteract();
		}
		//update velocity and move character
		this.Velocity = inputDirection * moveSpeed;	
		MoveAndSlide();
	
		UpdateAnimation();
	}
	
	public void ExecuteInteract(){
		if(currentInteract!=null){
			//check which type of interaction is executed
		 switch(currentInteract.GetInteractType()){
			case "collect":
				//check if a keycard or a weapon is picked up
				if(currentInteract.GetValue()=="keycard"){
						this.gotKeycard = true;
						//update interaction labels and visability of keycard
						GetParent().GetNode<StaticBody2D>("keycard").Visible = false;
						GetParent().GetNode<StaticBody2D>("keycard").GetNode<InteractionArea>("InteractionArea").SetInteractType("collected");
						GetParent().GetNode<StaticBody2D>("keycard").GetNode<InteractionArea>("InteractionArea").SetLabel("");
						//change label of door
						GetParent().GetNode<InteractionArea>("NextLevel").SetLabel("[E] to enter next level"); 
				}
				break;
			case "next_level":
				if(gotKeycard){
					GetTree().ChangeSceneToFile("res://levels/" + GetParent().GetNode<InteractionArea>("NextLevel").GetValue());
				}
				break;
			}
		}
	}


	public void UpdateAnimation(){
		if(this.Velocity!=Vector2.Zero){
			stateMachine.Travel("walk");
		}
		else{
			stateMachine.Travel("idle");
		}
	}

	private void _on_interaction_area_area_entered(InteractionArea area)
	{
		this.currentInteract = area;
		this.currentInteract.GetNode<Label>("Label").Text = currentInteract.GetLabel();
	}

	private void _on_interaction_area_area_exited(InteractionArea area)
	{
		area.GetNode<Label>("Label").Text = "";
		this.currentInteract = null;
	}
	
	public async void Fire(){
		Node bulletInstance = bullet.Instantiate();
		GetParent().AddChild(bulletInstance);
		canFire = false;
		await ToSignal(GetTree().CreateTimer(firerate), "timeout");
		canFire = true;
	}
	
	public Vector2 GetPosition(){
		return Position;
	}
	
}
