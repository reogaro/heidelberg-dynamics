using Godot;
using System;

public partial class InteractionArea : Area2D
{
	[Export]
	private String interactionLabel = "none";
	[Export]
	private String interactionValue = "none";
	[Export]
	private String interactionType = "none";
	
	public String GetLabel(){
		return interactionLabel;
	}
	public String GetInteractType(){
		return interactionType;
	}
	public String GetValue(){
		return interactionValue;
	}
	public void SetLabel(String newLabel){
		interactionLabel = newLabel;
	}
	public void SetInteractType(String newType){
		interactionType = newType;
	}
	public void SetValue(String newValue){
		interactionValue = newValue;
	}
	
	public override void _Ready(){
		GetNode<NinePatchRect>("DialogueManager").Visible = false;
	}

	
	public void StartDialogue(String dialogueNumber){
		GetNode<NinePatchRect>("DialogueManager").Visible = true;
		switch(dialogueNumber){
			case "0":
				DisplayDialogue("Info!","Welcome to Heidelberg Dynamics! You are L41k4, and need to escape this facility."
									+ " Use [W][A][S][D] to move to the next room. Press [E] to continue.");
				break;
			case "1":
				DisplayDialogue("Info!","You need to be careful. This facility is full of enemie turrets that will" +
									" shoot you on sight. Use [LMB] to shoot your weapon.");
				break;
			case "2":
				DisplayDialogue("Info!","You need to collect a keycard to open the door to the next level.");
				break;
			default:
				EndDialogue();
				break;
		}
	}
	
	public void DisplayDialogue(String name,String text){
		GetNode<NinePatchRect>("DialogueManager").GetNode<RichTextLabel>("Name").Text = name;
		GetNode<NinePatchRect>("DialogueManager").GetNode<RichTextLabel>("Text").Text = text;
	}
	
	public void EndDialogue(){
		GetNode<NinePatchRect>("DialogueManager").Visible = false;
		QueueFree();
	}

}

