using Godot;
using System;
using System.Threading;

public partial class FareDodger_AS : CharacterBody2D
{
    // Node references
    AnimatedSprite2D Sprite;
    RayCast2D bottomLeft;
    RayCast2D bottomRight;

    // Variables
    private Vector2 velocity;
    private int gravity = 2000;
    private int speed = 190;
    private float oscillationPeriod = 3.0f;
    private float minX, maxX;
    private bool isOnGround;

    private bool isAttacking = false;  // Flag to track if the attack is in progress

    private bool isTakingDamage = false;
    // Health properties
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }

    // Signal for death event
    [Signal] public delegate void DeathEventHandler();
    [Signal] public delegate void HealthChangedEventHandler();

    public override void _Ready()
    {
        // Initialize nodes
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        bottomLeft = GetNode<RayCast2D>("LeftRaycast");
        bottomRight = GetNode<RayCast2D>("RightRaycast");

        // Connect the "animation_finished" signal safely
        if (Sprite != null)
        {
            // Disconnect any previous connection to avoid redundant connections
            if (Sprite.IsConnected("animation_finished", new Callable(this, "_on_animated_sprite_2d_animation_finished_fd")))
            {
                Sprite.Disconnect("animation_finished", new Callable(this, "_on_animated_sprite_2d_animation_finished_fd"));
            }

            // Connect the "animation_finished" signal
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
        minX = -30;
        maxX = 30;

        MaxHealth = 50;
        CurrentHealth = MaxHealth;
    }

    // Signal handler when an animation finishes
    public void _on_animated_sprite_2d_animation_finished_fd(string animation_name)
    {
        if (animation_name == "Death") // When the death animation finishes
        {
            GD.Print("Enemy killed after death animation");
            EmitSignal(nameof(Death));  // Emit the death signal
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
    int previousHealth = CurrentHealth; // Store the health before change
    CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    EmitSignal(nameof(HealthChangedEventHandler));

    if (amount < 0 && previousHealth > CurrentHealth)  // Only if health is lost
    {
        GD.Print("Enemy TakeDamage Animation");
        Sprite.Play("TakeDamage");

        // Ensure animation doesn't get stuck by forcing a return to "Move"
        GetTree().CreateTimer(0.5f).Connect("timeout", new Callable(this, "_on_takedamage_animation_finished"));
    }

    if (CurrentHealth <= 0)
    {
        CurrentHealth = 0;
        GD.Print("Enemy has died");
        Sprite.Play("Death");

        SetPhysicsProcess(false);
        SetProcess(false);

        GetTree().CreateTimer(2.0f).Connect("timeout", new Callable(this, "_on_death_animation_finished"));
    }
    }


    // This method is called after the death animation has finished
    private void _on_death_animation_finished()
    {
        GD.Print("Enemy removed after death animation.");
        QueueFree();  // Remove the enemy from the scene
    }

    // Called every frame
    public override void _PhysicsProcess(double delta)
    {
        // Transition to "Move" animation only if we're not attacking and on the ground
        if (isAttacking && Sprite.Animation != "Attack2" && isOnGround)
        {
            GD.Print("not attacking");
            Sprite.Play("Move");
        }

        // AI-controlled movement (moving back and forth)
        isOnGround = bottomLeft.IsColliding() || bottomRight.IsColliding();

        if (isOnGround)
        {
                    // Check for wall collisions using RayCast2D
        bool isFacingWallLeft = bottomLeft.IsColliding();
        bool isFacingWallRight = bottomRight.IsColliding();

        if (isFacingWallLeft || isFacingWallRight)
        {
            // Reverse the movement direction when colliding with a wall
            velocity.X = -velocity.X;
            GD.Print("Wall detected! Reversing direction.");
        }
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
        // Skip attack if the enemy is dead
        if (CurrentHealth <= 0)
        {
            GD.Print("Enemy is dead, no attack will happen.");
            return;  // Exit the function early if the enemy is dead
        }

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
            if(Sprite.Animation == "TakeDamage")
            {
                SpriteFrames spriteFrames = Sprite.SpriteFrames;  // Correct property name in Godot 4.x
                int frameCount = spriteFrames.GetFrameCount("TakeDamage");  // Get frame count for the Attack2 animation
                float fps = (float)Sprite.SpriteFrames.GetAnimationSpeed("TakeDamage");  // Correct way to get the fps in Godot 4.x
                float attackDuration = frameCount / fps;  // Calculate the duration of the attack animation

                // Create a timer to wait for the animation to finish
                GetTree().CreateTimer(attackDuration).Connect("timeout", new Callable(this, "_on_takedamage_animation_finished"));
            }

            // Delay after the attack animation, then trigger death sequence
            if (Sprite.Animation == "Attack2")
            {
                SpriteFrames spriteFrames = Sprite.SpriteFrames;  // Correct property name in Godot 4.x
                int frameCount = spriteFrames.GetFrameCount("Attack2");  // Get frame count for the Attack2 animation
                float fps = (float)Sprite.SpriteFrames.GetAnimationSpeed("Attack2");  // Correct way to get the fps in Godot 4.x
                float attackDuration = frameCount / fps;  // Calculate the duration of the attack animation

                // Create a timer to wait for the animation to finish
                GetTree().CreateTimer(attackDuration).Connect("timeout", new Callable(this, "_on_attack_animation_finished"));
            }
        }
        else if (body.IsInGroup("bullets"))
            {
                 GD.Print("Enemy hit by bullet: " + body.Name);

                if (!isTakingDamage) // Only process damage if not already taking damage
                {
                    isTakingDamage = true;  // Prevent repeated damage triggering
                    ChangeHealth(-25);  // Reduce health
                }

                body.QueueFree(); 
            }
        else
        {
            GD.Print("Not a PlayerController. Detected: " + body.GetType());
        }
    }

    // This method is called after the attack animation finishes and the delay has elapsed
    private void _on_attack_animation_finished()
    {
        GD.Print("Starting death sequence after attack animation delay");
        Sprite.Play("Move");  // Play the move animation
        GD.Print("Set the isAttacking back to false");

        isAttacking = false;
    }


    private void _on_takedamage_animation_finished()
    {
        if (CurrentHealth > 0) // Only transition if still alive
        {
            GD.Print("TakeDamage animation finished. Resuming movement.");
            isTakingDamage = false;  // Allow movement again
            Sprite.Play("Move");  
        }
    }

    // public void RespawnEnemy()
    // {
    // Reset health, state, or any other properties here
    //     CurrentHealth = MaxHealth; // Restore enemy health
    //     GD.Print("Enemy respawned");

    // You may want to reset some other properties, like animation, position, etc.
    //     Sprite.Play("Move");  // Assuming the enemy has an idle animation
    //     SetPhysicsProcess(true);  // Enable physics processing for the enemy
    // }
}