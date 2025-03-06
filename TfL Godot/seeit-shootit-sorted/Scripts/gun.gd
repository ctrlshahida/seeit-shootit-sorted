extends Area2D
@onready var gun_shot_sound: AudioStreamPlayer2D = $GunShotSound

@export var speed = 600
var direction = Vector2.RIGHT  # Default direction is to the right
static var gun_shot_counter = 0

func _ready():
	add_to_group("bullets")
	set_direction(direction)
	connect("area_entered", Callable(self, "_on_area_entered"))
	gun_shot_counter += 1
	
	# Only play sound if counter is greater than 1
	if gun_shot_sound and gun_shot_counter > 1:
		gun_shot_sound.play()
		
func set_direction(dir: Vector2):
	direction = dir

func _physics_process(delta):
	global_position.x += direction.x * speed * delta

func _on_visible_on_screen_notifier_2d_screen_exited():
	queue_free()
	
func _on_area_entered(area: Area2D):
	print("Bullet hit:", area.name)
	
	var enemy = area.get_parent()  
	
	if enemy and enemy.has_method("ChangeHealth") and enemy.CurrentHealth > 0:  # Check if the enemy has a ChangeHealth method
		print("Bullet hit FareDodger_AS!")
		enemy.ChangeHealth(-25)  
		queue_free()  
	elif enemy and enemy.has_method("ChangeHealthBoss"):
		enemy.ChangeHealthBoss(-10)
		queue_free()
	else:
		print("Hit something else:", area.name)
