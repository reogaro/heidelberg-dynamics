using Godot;
using System;

public partial class DialogueManager : Node2D
{
	
	public override void _Ready(){
		Visible = true;
	}

	
	public void StartDialogue(String dialogueNumber){
		//GetParent().SetMoveable(false);
		Visible = false;
		switch(dialogueNumber){
			case "0":
				DisplayDialogue("Game","Welcome to Heidelberg Dynamics! You are L41k4, and need to escape this facility."
									+ " Use [W][A][S][D] to move to the next room. Press [E] to continue");
				break;
			case "1":
				DisplayDialogue("Game","You need to be careful. This facility is full of enemie turrets that will" +
									" shoot you on sight. Use [LMB] to shoot your weapon");
				break;
			default:
				EndDialogue();
				break;
		}
	}
	
	public void DisplayDialogue(String name,String text){
		GetNode<RichTextLabel>("Name").Text = name;
		GetNode<RichTextLabel>("Text").Text = text;
	}
	
	public void EndDialogue(){
		Visible = false;
		//GetParent().SetMoveable(true);
	}
	
	private void _Input(){
		if(Input.IsActionPressed("continue")){
			EndDialogue();
		}
	}
	
}
