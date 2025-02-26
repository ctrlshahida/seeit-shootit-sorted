using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // Get the sprite node
    }

    public const float AirControlFactor = 0.5f; // The factor to reduce horizontal speed while in the air

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
        if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get input direction for movement
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

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

            // Flip the sprite depending on movement direction
            _sprite.FlipH = direction.X < 0;
        }
        else if (IsOnFloor()) // If standing still on the ground, play idle animation
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            _sprite.Play("idle");
        }

        // Assign and move
        Velocity = velocity;
        MoveAndSlide();
    }



}
