using Godot;
using System;

public partial class Player : CharacterBody2D
{
	//private float moveSpeed = 175;
	private float moveSpeed = 300;
	private bool gotKeycard = false;
	private bool gotExtraCard = true;
	private bool moveable = true;
	private InteractionArea currentInteract;
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback stateMachine;
	private double firerate = 0.5;
	private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");
	private bool canFire = true;
	private Health health = new Health(1000,1000,100,100,100,100);
	
	public override void _Ready(){
		this.gotKeycard = false;
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}
	
	public override void _PhysicsProcess(double delta){
		GetParent().GetNode<CanvasLayer>("HUD").GetNode<ProgressBar>("Health").Value = health._hp;
		GetParent().GetNode<CanvasLayer>("HUD").GetNode<ProgressBar>("Shield").Value = health._shield;
		//get input directions
		Vector2 inputDirection = new Vector2(
			Input.GetActionRawStrength("right") - Input.GetActionRawStrength("left"),
			Input.GetActionRawStrength("down") - Input.GetActionRawStrength("up")
		);
		if(moveable){
			LookAt(GetGlobalMousePosition());

			//fire bullet if pressed
			if(Input.IsActionPressed("fire")&& canFire){
				Fire();
			}
			//update velocity and move character
			this.Velocity = inputDirection * moveSpeed;	
			MoveAndSlide();
	
			UpdateAnimation();
		}
		//execute interaction if needed
		if(Input.IsActionPressed("interact")){
			ExecuteInteract();
		}

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
						if(gotExtraCard){
							GetParent().GetNode<InteractionArea>("NextLevel").SetLabel("[E] to enter next level"); 
						}
				}
				else if(currentInteract.GetValue()=="keycard2"){
						this.gotExtraCard = true;
						
						//update interaction labels and visability of keycard
						GetParent().GetNode<StaticBody2D>("keycard2").Visible = false;
						GetParent().GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetInteractType("collected");
						GetParent().GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetLabel("");
						//change label of door
						if(gotKeycard){
							GetParent().GetNode<InteractionArea>("NextLevel").SetLabel("[E] to enter next level");
						} 
				}
				break;
			case "next_level":
				if(gotKeycard && gotExtraCard){
					GetTree().ChangeSceneToFile("res://levels/" + GetParent().GetNode<InteractionArea>("NextLevel").GetValue());
				}
				break;
			case "dialogue":
				moveable = true;
				currentInteract.EndDialogue();
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
		if(area.GetInteractType()=="dialogue"){
			area.StartDialogue(area.GetValue());
			moveable = false;
		}
	}

	private void _on_interaction_area_area_exited(InteractionArea area)
	{
		area.GetNode<Label>("Label").Text = "";
		this.currentInteract = null;
	}
	
	public async void Fire(){
		GetNode<AudioStreamPlayer>("BulletSound").Play();
		RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
		GetParent().AddChild(bulletInstance);
		bulletInstance.Position = GetNode<Node2D>("BulletPoint").GlobalPosition;
		bulletInstance.RotationDegrees = RotationDegrees;
		bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation),new Vector2());
		canFire = false;
		await ToSignal(GetTree().CreateTimer(firerate), "timeout");
		canFire = true;
	}
	
	public Vector2 GetPosition(){
		return Position;
	}
	
	public void SetMoveable(bool val){
		moveable = val;
	}
	
	public void SetExtraCard(bool val){
		gotExtraCard = val;
	}
}
