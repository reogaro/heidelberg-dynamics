using Godot;
using System;

/// <summary>
/// This class represents the player character in the game.
/// </summary>
public partial class Player : CharacterBody2D
{
	#region Fields

	/// <summary>
	/// The player's movement speed.
	/// </summary>
	private float moveSpeed = 175;

	/// <summary>
	/// Indicates if the player has collected the keycard.
	/// </summary>
	private bool gotKeycard = false;

	/// <summary>
	/// Indicates if the player has collected the extra keycard.
	/// </summary>
	private bool gotExtraCard = true;

	/// <summary>
	/// Determines if the player can move.
	/// </summary>
	private bool moveable = true;

	/// <summary>
	/// Keeps track of the number of bosses defeated.
	/// </summary>
	private int bossCount = 0;

	/// <summary>
	/// Reference to the currently interacted InteractionArea.
	/// </summary>
	private InteractionArea currentInteract;

	/// <summary>
	/// Reference to the player's AnimationTree.
	/// </summary>
	private AnimationTree animationTree;

	/// <summary>
	/// Reference to the AnimationNodeStateMachinePlayback node.
	/// </summary>
	private AnimationNodeStateMachinePlayback stateMachine;

	/// <summary>
	/// The fire rate of the player's weapon in seconds.
	/// </summary>
	private double firerate = 0.5;

	/// <summary>
	/// PackedScene reference to the projectile scene.
	/// </summary>
	private PackedScene bullet = GD.Load<PackedScene>("res://projectiles/projectile.tscn");

	/// <summary>
	/// Indicates if the player can fire.
	/// </summary>
	private bool canFire = true;

	#endregion

	/// <summary>
	/// Initializes the player character.
	/// </summary>
	public override void _Ready()
	{
		moveable = true;
		this.gotKeycard = false;
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");  

	}

	/// <summary>
	/// Handles the player's physics processing and reacts to player inputs.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _PhysicsProcess(double delta)
	{
		// Update health bars based on PlayerHealth node
		GetParent().GetNode<CanvasLayer>("HUD").GetNode<ProgressBar>("Health").Value = GetNode<PlayerHealth>("Health").health._hp;
		GetParent().GetNode<CanvasLayer>("HUD").GetNode<ProgressBar>("Shield").Value = GetNode<PlayerHealth>("Health").health._shield;

		// Check if player is dead and update visuals accordingly
		if (!GetNode<PlayerHealth>("Health").health._alive)
		{
			moveable = false;
			GetParent().GetNode<CanvasLayer>("HUD").GetNode<NinePatchRect>("Defeat").Visible = true;
		}

		// Get input directions
		Vector2 inputDirection = new Vector2(
			Input.GetActionRawStrength("right") - Input.GetActionRawStrength("left"),
			Input.GetActionRawStrength("down") - Input.GetActionRawStrength("up")
		);

		if (moveable)
		{
			// Look at the mouse position
			LookAt(GetGlobalMousePosition());

			// Fire bullet if pressed and able to fire
			if (Input.IsActionPressed("fire") && canFire)
			{
				Fire();
			}

			// Update velocity and move character
			this.Velocity = inputDirection * moveSpeed;
			MoveAndSlide();

			UpdateAnimation();
		}

		// Execute interaction if needed
		if (Input.IsActionPressed("interact"))
		{
			if (GetNode<PlayerHealth>("Health").health._alive)
			{
				ExecuteInteract();
			}
			else
			{
				GetTree().ChangeSceneToFile("res://levels/level_1.tscn");
			}
		}

		// Handle other input actions
		if (Input.IsActionPressed("exit"))
		{
			GetTree().Quit();
		}

		if (Input.IsActionPressed("cheat"))
		{
			GetTree().ChangeSceneToFile("res://levels/level_6.tscn");
		}

		if (Input.IsActionPressed("speed"))
		{
			moveSpeed = 500;
		}

		// Check for endgame condition
		if (bossCount == 2)
		{
			EndGame();
		}
	}

	/// <summary>
	/// Determines and executes the current interaction, if any.
	/// </summary>
	public void ExecuteInteract()
	{
		if (currentInteract != null)
		{
			// Check which type of interaction is executed
			switch (currentInteract.GetInteractType())
			{
				case "collect":
					// Check if a keycard or a weapon is picked up
					if (currentInteract.GetValue() == "keycard")
					{
						this.gotKeycard = true;
						GetParent().GetNode<CanvasLayer>("HUD").GetNode<Sprite2D>("Sprite2D").Visible = true;

						// Update interaction labels and visability of keycard
						GetParent().GetNode<StaticBody2D>("keycard").Visible = false;
						GetParent().GetNode<StaticBody2D>("keycard").GetNode<InteractionArea>("InteractionArea").SetInteractType("collected");
						GetParent().GetNode<StaticBody2D>("keycard").GetNode<InteractionArea>("InteractionArea").SetLabel("");

						// Change label of door
						if (gotExtraCard)
						{
							GetParent().GetNode<InteractionArea>("NextLevel").SetLabel("[E] to enter next level");
						}
					}
					else if (currentInteract.GetValue() == "keycard2")
					{
						this.gotExtraCard = true;
						GetParent().GetNode<CanvasLayer>("HUD").GetNode<Sprite2D>("Sprite2D2").Visible = true;

						// Update interaction labels and visability of keycard
						GetParent().GetNode<StaticBody2D>("keycard2").Visible = false;
						GetParent().GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetInteractType("collected");
						GetParent().GetNode<StaticBody2D>("keycard2").GetNode<InteractionArea>("InteractionArea").SetLabel("");

						// Change label of door
						if (gotKeycard)
						{
							GetParent().GetNode<InteractionArea>("NextLevel").SetLabel("[E] to enter next level");
						}
					}
					break;
				case "next_level":
					if (gotKeycard && gotExtraCard)
					{
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

	/// <summary>
	/// Updates the player's animation based on their velocity.
	/// </summary>
	public void UpdateAnimation()
	{
		if (this.Velocity != Vector2.Zero)
		{
			stateMachine.Travel("walk");
		}
		else
		{
			stateMachine.Travel("idle");
		}
	}

	/// <summary>
	/// Handles when the player enters an InteractionArea and displays the interaction information.
	/// </summary>
	/// <param name="area">The InteractionArea the player entered.</param>
	private void _on_interaction_area_area_entered(InteractionArea area)
	{
		if (area.GetInteractType() != "heal")
		{
			this.currentInteract = area;
			this.currentInteract.GetNode<Label>("Label").Text = currentInteract.GetLabel();
			if (area.GetInteractType() == "dialogue")
			{
				area.StartDialogue(area.GetValue());
				moveable = false;
			}
		}
		else
		{
			GetNode<PlayerHealth>("Health").health.HealBoth(500);
			area.GetParent().QueueFree();
		}
	}

	/// <summary>
	/// Handles when the player exits an InteractionArea and resets the according values.
	/// </summary>
	/// <param name="area">The InteractionArea the player exited.</param>
	private void _on_interaction_area_area_exited(InteractionArea area)
	{
		area.GetNode<Label>("Label").Text = "";
		this.currentInteract = null;
	}

	/// <summary>
	/// Fires the player's weapon by creating a new projectile instance.
	/// </summary>
	public async void Fire()
	{
		GetNode<AudioStreamPlayer>("BulletSound").Play();

		RigidBody2D bulletInstance = bullet.Instantiate<RigidBody2D>();
		GetParent().AddChild(bulletInstance);

		bulletInstance.Position = GetNode<Node2D>("BulletPoint").GlobalPosition;
		bulletInstance.RotationDegrees = RotationDegrees;
		bulletInstance.ApplyImpulse(new Vector2(750, 0).Rotated(Rotation), new Vector2());

		canFire = false;
		await ToSignal(GetTree().CreateTimer(firerate), "timeout");
		canFire = true;
	}

	/// <summary>
	/// Gets the player's current position.
	/// </summary>
	/// <returns>The player's position.</returns>
	public Vector2 GetPosition()
	{
		return Position;
	}

	/// <summary>
	/// Sets the player's moveability.
	/// </summary>
	/// <param name="val">The new moveability state.</param>
	public void SetMoveable(bool val)
	{
		moveable = val;
	}

	/// <summary>
	/// Sets the player's extra card state.
	/// </summary>
	/// <param name="val">The new extra card state.</param>
	public void SetExtraCard(bool val)
	{
		gotExtraCard = val;
	}

	/// <summary>
	/// Increases the boss count.
	/// </summary>
	public void IncreaseBossCount()
	{
		bossCount++;
	}

	/// <summary>
	/// Ends the game after a 10 second delay.
	/// </summary>
	public async void EndGame()
	{
		await ToSignal(GetTree().CreateTimer(10), "timeout");
		GetTree().ChangeSceneToFile("res://levels/level_7.tscn");
	}
}
