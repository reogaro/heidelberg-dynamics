using Godot;
using System;

public partial class DialogueManager : Node2D
{

	private bool active = false;
	private int currentLine = -1;
	private Godot.Collections.Dictionary dialogue;
	
	public override void _Ready(){
		GetNode<INinePatchRect>("NinePatchRect").Visible = false;
	}

	
	public void StartDialogue(String filename){
		active = true;
		GetNode<NinePatchRect>("NinePatchRect").Visible = true;
		dialogue = LoadDialogue(filename);
		NextLine();
	}
	
	public Godot.Collections.Dictionary LoadDialogue(String filename){
		Godot.Collections.Dictionary data;
		File.ReadAllText("res://dialogues" + filename);
		JSON jsonLoader = new JSON();
		jsonLoader.Parse(data);
		Godot.Collections.Dictionary dataDict = (Godot.Collections.Dictionary)jsonLoader.Data
		return data;
	}
	
	private override void _Input(){
		if(Input.IsActionPressed("continue") && active){
			NextLine();
		}
	}
	
	private void NextLine(){
		currentLine++;
		if(currentLine >= dialogue.size()){
			active = false;
			GetNode<NinePatchRect>("NinePatchRect").Visible = false;
			currentLine = -1;
			return;
		}
		GetNode<NinePatchRect>("NinePatchRect").GetNode<RichTextLabel>("Name").Text = dialogue[currentLine]["name"];
		GetNode<NinePatchRect>("NinePatchRect").GetNode<RichTextLabel>("Text").Text = dialogue[currentLine]["text"];
	}
}
