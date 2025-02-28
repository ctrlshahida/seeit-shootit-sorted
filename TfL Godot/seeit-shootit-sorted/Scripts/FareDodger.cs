using Godot;

public partial class FareDodger : CharacterBody2D
{
    Sprite2D Sprite;
    AnimationPlayer animationPlayer ;
    RayCast2D bottomLeft;
    RayCast2D bottomRight;

    private Vector2 velocity;
    private int gravity = 2000;
    private int speed = 180;
    private float oscillationPeriod = 3.0f;
    private float minX, maxX; // Minimum and maximum X position values
    private bool isOnGround;
    // Called when the node enters the scene tree for the first time.
public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite");
        bottomLeft = GetNode<RayCast2D>("LeftRaycast");
        bottomRight = GetNode<RayCast2D>("RightRaycast");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        velocity.X = speed;
        // Set minX and maxX values based on your desired movement range
        minX = -10; // Adjust as needed
        maxX =10; // Adjust as needed

       // Connect the body_entered signal from the Area2D child node to the enemy_contact() method
        // Correct signal connection syntax
        // var enemyArea = GetNode<Area2D>("EnemyArea2D");
        // enemyArea.Connect("body_entered", new Callable(this, "enemy_contact"));
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _PhysicsProcess(double delta)
    {
        // Play "Move" animation when the node is ready
        // Replace with the path to your AnimationPlayer node
        animationPlayer.Play("Move");
        // AI-controlled movement (example: moving back and forth)
        // Check if the character is on the ground using raycasts
        isOnGround = bottomLeft.IsColliding() || bottomRight.IsColliding();

        // AI-controlled movement (example: moving back and forth)
        if (isOnGround)
        {
            float timeInSeconds = Time.GetTicksMsec() / 1000.0f;
            float oscillationValue = Mathf.Sin(timeInSeconds / (float)oscillationPeriod);
            Vector2 movementDirection = new Vector2(oscillationValue, 0);
            if (movementDirection != Vector2.Zero)
            {
                movementDirection = movementDirection.Normalized();
                velocity = movementDirection * new Vector2(speed, 0);
            }

            // Reverse movement direction when reaching the edge of the range
            if (Position.X <= minX || Position.X >= maxX)
            {
                velocity.X = -velocity.X;
            }
        }
                else
        {
            // Apply gravity when not on the ground
            velocity.Y += (float)(gravity * delta);
        }

        // Flip the sprite based on the movement direction
        Sprite.FlipH = velocity.X < 0;

        // Apply gravity
        velocity.Y += (float)(gravity * delta);
        if (velocity.Y > gravity)
        {
            velocity.Y = gravity;
        }
        velocity.X = Mathf.Clamp(velocity.X, minX, maxX);
        Velocity = velocity;
        MoveAndSlide();
    }

    public void _on_enemy_area_2d_body_entered(Node2D body)
    {
        if(body is PlayerController)
        {
            GD.Print("Body: " + body + " hurt by enemy");
            PlayerController pc = body as PlayerController;
            pc.ChangeHealth(-25);
        }
    }
}
