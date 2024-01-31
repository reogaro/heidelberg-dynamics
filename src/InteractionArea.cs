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
}
