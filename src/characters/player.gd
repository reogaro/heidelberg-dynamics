extends CharacterBody2D

@export var move_speed : float = 250
@export var max_health : float = 100
var current_health = max_health
var alive = true

# Decreases the current health by the specified amount
func takeDamage(amount: float):
	current_health = max(0, current_health - amount)
	if(current_health == 0):
		die()
		
func setHealth(value: float):
	current_health = clamp(value, 0, max_health)

# Increases the current health by the specified amount, up to the maximum health
func heal(amount: float):
	current_health = min(max_health, current_health + amount)

# Returns the current health
func getCurrentHealth() -> float:
	return current_health

# Returns the maximum health
func getMaxHealth() -> float:
	return max_health

func die():
	print("You are ded. Not big soup rice.")
	alive = false

func _physics_process(_delta):
	# get input directions
	var input_direction = Vector2(
		Input.get_action_raw_strength("right") - Input.get_action_raw_strength("left"),
		Input.get_action_raw_strength("down") - Input.get_action_raw_strength("up")
	)
	
	velocity = input_direction * move_speed
	
	move_and_slide()
