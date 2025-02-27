using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    private AnimatedSprite2D _sprite;
<<<<<<< HEAD
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    [Signal] public delegate void HealthChangedEventHandler();
    [Signal] public delegate void DeathEventHandler();
=======
>>>>>>> 1e7b9f2 (Change file Directory)

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // Get the sprite node
<<<<<<< HEAD

        MaxHealth = 100;
        CurrentHealth = MaxHealth;
    }

        public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_fullscreen_toggle"))
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen
                ? DisplayServer.WindowMode.Windowed
                : DisplayServer.WindowMode.Fullscreen);
        }
    }

    public void ChangeHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        EmitSignal("HealthChanged");

        if(CurrentHealth <= 0){
            CurrentHealth = 0;
            _sprite.Play("death");
            GD.Print("Player has died");
            _sprite.AnimationFinished += _on_animated_sprite_2d_animation_finished;
            SetPhysicsProcess(false); // Stop player movement
            SetProcess(false);
            // EmitSignal(nameof(Death));
        }
    }

    private void _on_animated_sprite_2d_animation_finished()
    {
        if (_sprite.Animation == "death") // Ensure we're checking the correct animation
        {
            GD.Print("Respawning player after death animation");

            // Disconnect the signal to prevent multiple calls
            _sprite.AnimationFinished -= _on_animated_sprite_2d_animation_finished;

            EmitSignal(nameof(Death));
        }
    }

    public const float AirControlFactor = 0.5f; // The factor to reduce horizontal speed while in the air

=======
    }

>>>>>>> 1e7b9f2 (Change file Directory)
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Get the default gravity value from project settings
        float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

        // Apply gravity if not on the floor
        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        // Handle Jump
<<<<<<< HEAD
        if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
=======
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
>>>>>>> 1e7b9f2 (Change file Directory)
        {
            velocity.Y = JumpVelocity;
        }

        // Get input direction for movement
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

<<<<<<< HEAD
        // Apply horizontal movement with reduced speed if in the air
        if (direction != Vector2.Zero)
        {
            if (IsOnFloor()) // On the ground, move at full speed
            {
                velocity.X = direction.X * Speed;
                _sprite.Play("run");
            }
            else // In the air, reduce horizontal movement speed
            {
                velocity.X = direction.X * Speed * AirControlFactor;
                _sprite.Play("jump");
            }
=======
        if (!IsOnFloor()) // If the player is in the air, play jump animation
        {
            _sprite.Play("jump");
        }
        else if (direction != Vector2.Zero) // If moving on the ground, play run animation
        {
            velocity.X = direction.X * Speed;
            _sprite.Play("run");
>>>>>>> 1e7b9f2 (Change file Directory)

            // Flip the sprite depending on movement direction
            _sprite.FlipH = direction.X < 0;
        }
<<<<<<< HEAD
        else if (IsOnFloor()) // If standing still on the ground, play idle animation
=======
        else // If standing still on the ground, play idle animation
>>>>>>> 1e7b9f2 (Change file Directory)
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            _sprite.Play("idle");
        }
<<<<<<< HEAD
=======

>>>>>>> 1e7b9f2 (Change file Directory)
        // Assign and move
        Velocity = velocity;
        MoveAndSlide();
    }
<<<<<<< HEAD

    public void TakeDamage(){
        GD.Print("Player has taken damage");
    }

    public void RespawnPlayer(){
        GD.Print("Player respawned");
        CurrentHealth = MaxHealth;
        EmitSignal("HealthChanged");

        //_sprite.Play("idle");
        SetPhysicsProcess(true);
        SetProcess(true);
    }
=======
>>>>>>> 1e7b9f2 (Change file Directory)
}
