# test_player.gd

extends GutTest
var playerload = load("res://characters/player.gd")
var _player = null

func before_each():
	_player = playerload.new()

func after_each():
	_player.free()

func test_nothing():
	pass_test('nothing tested, passing')

func test_healing():
	var testDamage = 50
	var testHeal = 30
	assert_true(testDamage > testHeal, "Overheal gets tested seperately")
	_player.setHealth(_player.max_health - testDamage)
	_player.heal(testHeal)
	assert_eq(_player.getCurrentHealth(), _player.getMaxHealth() - testDamage + testHeal)
	_player.heal(testDamage)
	assert_true(_player.getCurrentHealth() <= _player.max_health, "Overheal")
	

# Function to test the takeDamage function
func test_TakeDamage():
	_player.setHealth(_player.max_health)
	var initial_health = _player.getCurrentHealth()
	var damage_amount = 20
	_player.takeDamage(damage_amount)
	
	# Check if health decreased by the correct amount
	assert_true(_player.getCurrentHealth() == initial_health - damage_amount, "Take damage test failed!")
	
	# Check if health doesn't go below zeros
	assert_true(_player.getCurrentHealth() >= 0, "Health should not be below zero!")
	
	print("Take damage test passed!")
