extends CharacterBody2D

#export variables
@export var move_speed : float = 250
@export var got_keycard = false
#onready variables
@onready var all_interacts = []
@onready var interact_label = $"interaction_components/Label"
@onready var animation_tree = $AnimationTree
@onready var state_machine = animation_tree.get("parameters/playback")

func _ready():
	got_keycard = false

func _physics_process(_delta):
	look_at(get_global_mouse_position())
	# get input directions
	var input_direction = Vector2(
		Input.get_action_raw_strength("right") - Input.get_action_raw_strength("left"),
		Input.get_action_raw_strength("down") - Input.get_action_raw_strength("up")
	)
	#execute interaction if needed
	if Input.is_action_just_pressed("interact"):
		execute_interact()
	#update velocity and move character
	velocity = input_direction * move_speed	
	move_and_slide()
	
	update_animation()

func execute_interact():
	if all_interacts:
		#check which type of interaction is executed
		match all_interacts[0].interaction_type:
			"collect":
				#check if a keycard or a weapon is picked up
				match all_interacts[0].interaction_value:
					"keycard":
						got_keycard = true
						#update interaction labels and visability of keycard
						get_parent().get_node("keycard").visible = false
						get_parent().get_node("keycard").get_node("interaction_area").interaction_type = "collected"
						get_parent().get_node("keycard").get_node("interaction_area").interaction_label = ""
						#change label of  
						get_parent().get_node("next_level").interaction_label = "[E] to enter next level"
						
						#update_interacts()
			"next_level":
				if got_keycard:
					get_tree().change_scene_to_file("res://levels/" + get_parent().get_node("next_level").interaction_value)
					
func _on_interaction_area_area_entered(area):
	all_interacts.insert(0,area)
	all_interacts[0].get_node("Label").text = all_interacts[0].interaction_label
	

func _on_interaction_area_area_exited(area):
	area.get_node("Label").text = ""
	all_interacts.erase(area)
	

#func update_interacts():
#	if all_interacts:
#		all_interacts[0].get_node("Label").text = all_interacts[0].interaction_label
#	else:
#		interact_label.text = ""

func update_animation():
	if velocity!=Vector2.ZERO:
		state_machine.travel("walk")
	else:
		state_machine.travel("idle")
