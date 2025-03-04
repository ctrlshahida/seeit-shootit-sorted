extends Area2D

@export var speed = 600
var direction = Vector2.RIGHT  # Default direction is to the right

func _ready():
	# Make sure the bullet direction is set when the gun is instantiated
	add_to_group("bullets")
	set_direction(direction)
	connect("area_entered", Callable(self, "_on_area_entered"))

func set_direction(dir: Vector2):
	# Set the direction of the bullet
	direction = dir

func _physics_process(delta):
	# Move the bullet in the direction it's facing
	global_position.x += direction.x * speed * delta

func _on_visible_on_screen_notifier_2d_screen_exited():
	queue_free()
	
func _on_area_entered(area: Area2D):
	print("Bullet hit:", area.name)
	
	# Ensure the area belongs to an enemy
	var enemy = area.get_parent()  # Get the parent (should be FareDodger_AS)
	
	if enemy and enemy.has_method("ChangeHealth"):  # Check if the enemy has a ChangeHealth method
		print("Bullet hit FareDodger_AS!")
		enemy.ChangeHealth(-25)  # Reduce enemy health by 10
		queue_free()  # Destroy the bullet after hitting the enemy
	else:
		print("Hit something else:", area.name)
