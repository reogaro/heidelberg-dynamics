using Godot;
using System;

/// <summary>
/// Represents an interaction area that triggers events when the player enters it.
/// </summary>
public partial class InteractionArea : Area2D
{
	/// <summary>
	/// The label displayed for the interaction area.
	/// </summary>
	[Export]
	private String interactionLabel = "none";

	/// <summary>
	/// The value associated with the interaction.
	/// </summary>
	[Export]
	private String interactionValue = "none";

	/// <summary>
	/// The type of interaction (e.g., "collect", "next_level", "dialogue").
	/// </summary>
	[Export]
	private String interactionType = "none";

	/// <summary>
	/// Gets the label displayed for the interaction area.
	/// </summary>
	/// <returns>The interaction label.</returns>
	public String GetLabel()
	{
		return interactionLabel;
	}

	/// <summary>
	/// Gets the type of interaction.
	/// </summary>
	/// <returns>The interaction type.</returns>
	public String GetInteractType()
	{
		return interactionType;
	}

	/// <summary>
	/// Gets the value associated with the interaction.
	/// </summary>
	/// <returns>The interaction value.</returns>
	public String GetValue()
	{
		return interactionValue;
	}

	/// <summary>
	/// Sets the label displayed for the interaction area.
	/// </summary>
	/// <param name="newLabel">The new label.</param>
	public void SetLabel(String newLabel)
	{
		interactionLabel = newLabel;
	}

	/// <summary>
	/// Sets the type of interaction.
	/// </summary>
	/// <param name="newType">The new interaction type.</param>
	public void SetInteractType(String newType)
	{
		interactionType = newType;
	}

	/// <summary>
	/// Sets the value associated with the interaction.
	/// </summary>
	/// <param name="newValue">The new interaction value.</param>
	public void SetValue(String newValue)
	{
		interactionValue = newValue;
	}

	/// <summary>
	/// Initializes the interaction area.
	/// </summary>
	public override void _Ready()
	{
		GetNode<NinePatchRect>("DialogueManager").Visible = false;
	}

	/// <summary>
	/// Starts a dialogue based on the given dialogue number.
	/// </summary>
	/// <param name="dialogueNumber">The dialogue number.</param>
	public void StartDialogue(String dialogueNumber)
	{
		GetNode<NinePatchRect>("DialogueManager").Visible = true;

		switch (dialogueNumber)
		{
			case "0":
				DisplayDialogue("Info!", "Welcome to Heidelberg Dynamics! You are L41k4, and need to escape this facility." +
										" Use [W][A][S][D] to move to the next room. Press [E] to continue.");
				break;
			case "1":
				DisplayDialogue("Info!", "You need to be careful. This facility is full of enemie turrets that will" +
										" shoot you on sight. Use [LMB] to shoot your weapon.");
				break;
			case "2":
				DisplayDialogue("Info!", "You need to collect a keycard to open the door to the next level.");
				break;
			case "3":
				DisplayDialogue("Info!", "You escaped! You can now enjoy your freedom.");
				break;
			default:
				EndDialogue();
				break;
		}
	}

	/// <summary>
	/// Displays a dialogue with the given name and text.
	/// </summary>
	/// <param name="name">The name to display.</param>
	/// <param name="text">The text to display.</param>
	public void DisplayDialogue(String name, String text)
	{
		GetNode<NinePatchRect>("DialogueManager").GetNode<RichTextLabel>("Name").Text = name;
		GetNode<NinePatchRect>("DialogueManager").GetNode<RichTextLabel>("Text").Text = text;
	}

	/// <summary>
	/// Ends the current dialogue and removes the dialogue manager.
	/// </summary>
	public void EndDialogue()
	{
		GetNode<NinePatchRect>("DialogueManager").Visible = false;
		QueueFree();
	}
}
