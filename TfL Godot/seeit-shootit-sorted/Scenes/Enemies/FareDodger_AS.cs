using Godot;
using System;

public partial class FareDodger_AS : CharacterBody2D
{
    // Node references
    AnimatedSprite2D Sprite;
    RayCast2D bottomLeft;
    RayCast2D bottomRight;

    // Variables
    private Vector2 velocity;
    private int gravity = 2000;
    private int speed = 180;
    private float oscillationPeriod = 3.0f;
    private float minX, maxX;
    private bool isOnGround;

    private bool isAttacking = false;  // Flag to track if the attack is in progress

    // Health properties
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }

    // Signal for death event
    [Signal] public delegate void DeathEventHandler();

    public override void _Ready()
    {
        // Initialize nodes
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        bottomLeft = GetNode<RayCast2D>("LeftRaycast");
        bottomRight = GetNode<RayCast2D>("RightRaycast");
        
            // Disconnect the signal only if it's already connected
        if (Sprite != null && Sprite.IsConnected("animation_finished", new Callable(this, "_on_animation_finished")))
        {
            Sprite.Disconnect("animation_finished", new Callable(this, "_on_animation_finished"));
        }
    	// Connect the "animation_finished" signal to the handler
		if (Sprite != null)
		{
			GD.Print("Connecting animation_finished signal...");
			Sprite.Connect("animation_finished", new Callable(this, "_on_animation_finished"));
		}
		else
		{
			GD.PrintErr("Sprite node is not found!");
		}

        // Set initial velocity
        velocity.X = speed;

        // Set range for movement
        minX = -40;
        maxX = 40;

        MaxHealth = 50;
        CurrentHealth = MaxHealth;
    }

    // Signal handler when an animation finishes
    public void _on_animation_finished(string animation_name)
    {
        if (animation_name == "Death") // When the death animation finishes
        {
            GD.Print("Enemy killed after death animation");
            EmitSignal(nameof(Death));
            QueueFree();  // Remove the enemy from the scene
        }

        if (animation_name == "Attack2") // After attack finishes
        {
            // Reset the attacking flag and transition to "Move"
            isAttacking = false;

            // If the enemy is on the ground, go back to the "Move" animation
            if (isOnGround)
            {
                GD.Print("Attack2 animation finished. Returning to Move.");
                Sprite.Play("Move");  // Transition to "Move" after attack
            }
        }
    }

    // Health change method
    public void ChangeHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        EmitSignal("HealthChanged");

        if (amount < 0)  // Only play TakeDamage animation if losing health
        {
            GD.Print("Enemy TakeDamage Animation");
            Sprite.Play("TakeDamage");
        }

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Sprite.Play("Death");
            GD.Print("Enemy has died");
            SetPhysicsProcess(false); // Stop player movement
            SetProcess(false);
        }
    }

    // Called every frame
    public override void _PhysicsProcess(double delta)
    {
        // Transition to "Move" animation only if we're not attacking and on the ground
        if (isAttacking && Sprite.Animation != "Attack2" && isOnGround)
        {
            GD.Print("not attacking");
            Sprite.Play("Move");
            // If we're not attacking, play the "Move" animation
            // if (Sprite.Animation != "Move")  // Ensure it's not already playing
            // {
            //     GD.Print("Switching to Move animation.");
            //     Sprite.Play("Move");  // Play "Move" animation
            // }
        }

        // AI-controlled movement (moving back and forth)
        isOnGround = bottomLeft.IsColliding() || bottomRight.IsColliding();

        if (isOnGround)
        {
            // Implement basic oscillation-based movement
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

        velocity.Y += (float)(gravity * delta);
        if (velocity.Y > gravity)
        {
            velocity.Y = gravity;
        }
        velocity.X = Mathf.Clamp(velocity.X, minX, maxX);
        Velocity = velocity;
        MoveAndSlide();
    }

    // Handle detection of the player and trigger attack animation
    public void _on_enemy_area_2d_body_entered_2(Node2D body)
    {
        if (body is PlayerController)
        {
            GD.Print("Detected body: " + body.GetType());
            GD.Print("Enemy: " + body + " hurt by enemy");

            // Check if the player is close enough to the enemy
            float distance = Position.DistanceTo(body.Position);
            if (distance < 40)  // Adjust the distance as needed
            {
                GD.Print("Player detected, triggering attack animation");

                // Stop any other animations and play the attack animation if not already playing
                if (!isAttacking && Sprite.Animation != "Attack2")
                {
                    GD.Print("Playing attack animation");
                    isAttacking = true;  // Set the attacking flag to true
                    Sprite.Play("Attack2");  // Play the attack animation
                }
            }

            // Deal damage to the player
            PlayerController pc = body as PlayerController;
            if (pc != null)
            {
                GD.Print("Player health changed by enemy attack");
                pc.ChangeHealth(-25);
            }
        }
        else
        {
            GD.Print("Not a PlayerController. Detected: " + body.GetType());
        }
		    // Add isAttacking reset here (though not ideal)
    		// isAttacking = false;
    }
}
