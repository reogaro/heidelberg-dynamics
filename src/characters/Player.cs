using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private float moveSpeed = 250;
	private bool gotKeycard = false;
	private Area2D allInteracts[];
	private AnimationTree animationTree;
	private StateMachine stateMachine;
	
	public override void _Ready(){
		this.gotKeycard = false;
	}
	
	public override void _PhyicsProcess(double delta){
		lookat(getGlobalMousePosition());
		//get input directions
		
		Vector2 inputDirection = Vector2(
		Input.getActionRawStrength("right") - Input.getActionRawStrength("left"),
		Input.getActionRawStrength("down") - Input.getActionRawStrength("up")
		);
		
		//execute interaction if needed
		if Input.isActionPressed("interact"){
			executeInteract();
		}
		//update velocity and move character
		this.Velocity = inputDirection * moveSpeed;	
		moveAndSlide();
	
		updateAnimation();
	}
	
	public void executeInteract(){
		if(allInteracts){
			//check which type of interaction is executed
			switch(allInteracts[0].getType()){
				case "collect":
					//check if a keycard or a weapon is picked up
					switch(allInteracts[0].getValue()){
						case "keycard":
							gotKeycard = true;
							//update interaction labels and visability of keycard
							getParent().getNode("keycard").visible = false;
							getParent().getNode("keycard").getNode("interaction_area").setType("collected");
							getParent().getNode("keycard").getNode("interaction_area").setLabel("");
							//change label of door
							getParent().getNode("next_level").interactionLabel("[E] to enter next level"); 
							
							//update_interacts()
							break;
					case "next_level":
						if(gotKeycard){
							getTree().changeSceneToFile("res://levels/" + getParent().getNode("next_level").getValue());
						}
						break;
					break;
			}
		}
	}
	
	public override void _onInteractionAreaAreaEntered(area){
		allInteracts.insert(0,area);
		allInteracts[0].getNode("Label").text = allInteracts[0].getLabel();
	}
	

	public override void _onInteractionAreaAreaExited(area){
		area.getNode("Label").text = "";
		allInteracts.erase(area);
	}	

	public void update_animation():
		if(velocity!=Vector2.ZERO){
			stateMachine.travel("walk");
		}
		else{
			stateMachine.travel("idle");
		}
}
