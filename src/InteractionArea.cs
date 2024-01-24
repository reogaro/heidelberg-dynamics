using Godot;
using System;

public class InteractionArea : Area2D
{
	private:
		String interactionLabel = "none"
		String interationValue = "none"
		String interactionType = "none"
	
	public String getLabel(){
		return interactionLabel;
	}
	public String getType(){
		return interactionType;
	}
	public String getValue(){
		return interactionValue;
	}
	public void setLabel(String newLabel){
		interactionLabel = newLabel;
	}
	public void setType(String newType){
		interactionType = newType;
	}
	public void setValue(String newValue){
		interactionValue = newValue;
	}
}
