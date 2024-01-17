extends CharacterBody2D

@export var move_speed : float = 250
@export var got_keycard = false
@onready var all_interacts = []
@onready var interact_label = $"interaction_components/Label"

func _ready():
	got_keycard = false

func _physics_process(_delta):
	# get input directions
	var input_direction = Vector2(
		Input.get_action_raw_strength("right") - Input.get_action_raw_strength("left"),
		Input.get_action_raw_strength("down") - Input.get_action_raw_strength("up")
	)
	if Input.is_action_just_pressed("interact"):
		execute_interact()
	velocity = input_direction * move_speed
	
	move_and_slide()

func execute_interact():
	if all_interacts:
		match all_interacts[0].interaction_type:
			"collect":
				match all_interacts[0].interaction_value:
					"keycard":
						got_keycard = true
						get_parent().get_node("keycard").visible = false
						get_parent().get_node("keycard").get_node("interaction_area").interaction_type = "collected"
						get_parent().get_node("keycard").get_node("interaction_area").interaction_label = ""
						get_parent().get_node("next_level").interaction_label = "[E] to enter next level"
						update_interacts()
			"next_level":
				if got_keycard:
					get_tree().change_scene_to_file("res://levels/" + get_parent().get_node("next_level").interaction_value)
func _on_interaction_area_area_entered(area):
	all_interacts.insert(0,area)
	update_interacts()

func _on_interaction_area_area_exited(area):
	all_interacts.erase(area)
	update_interacts()

func update_interacts():
	if all_interacts:
		interact_label.text = all_interacts[0].interaction_label
	else:
		interact_label.text = ""
