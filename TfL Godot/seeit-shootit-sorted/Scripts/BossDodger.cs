using Godot;
using System;

public partial class BossDodger : CharacterBody2D
{
    private AnimatedSprite2D _sprite;
    private enum BossState { Idle, Moving, Attacking }
    private BossState currentState = BossState.Idle;

    private const float Speed = 100.0f;
    private const float AttackRange = 100.0f;

    private PlayerController _player;
    private const float AttackCooldown = 1.0f;
    private float _attackCooldownTimer = 0.0f;

    public override void _Ready()
    {
        GD.Print("BossDodger is ready!");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetNode<PlayerController>("Player");

        currentState = BossState.Moving;
    }

    public override void _PhysicsProcess(double delta)
    {
        _attackCooldownTimer -= (float)delta;

        switch (currentState)
        {
            case BossState.Moving:
                MoveBehavior(delta);
                break;
            case BossState.Attacking:
                AttackBehavior(delta);
                break;
            default:
                break;
        }
    }

    private void MoveBehavior(double delta)
    {
        // Play the "Move" animation while moving
        _sprite.Play("Move");

        Vector2 direction = GetPlayerDirection();
        if (direction != Vector2.Zero)
        {
            // Normalize the direction vector, multiply by speed
            Vector2 velocity = direction.Normalized() * Speed;

            // Set the velocity and call MoveAndSlide()
            Velocity = velocity;
            MoveAndSlide();

            GD.Print("Boss is moving towards player");
        }

        // Transition to attacking state if player is in range
        if (IsPlayerInRange())
        {
            GD.Print("Player is within attack range. Transitioning to Attacking.");
            currentState = BossState.Attacking;
        }
    }

    private void AttackBehavior(double delta)
    {
        // Play the "Cleave" animation when attacking
        _sprite.Play("Cleave");

        if (_attackCooldownTimer <= 0.0f && IsPlayerInRange())
        {
            // Attack the player
            GD.Print("Attacking player!");
            _player.ChangeHealth(-25);
            _attackCooldownTimer = AttackCooldown;
        }

        // After the attack animation finishes, transition back to moving state
        if (_sprite.Animation == "Cleave" && !_sprite.IsPlaying())
        {
            GD.Print("Attack finished, transitioning to Moving.");
            currentState = BossState.Moving;
        }
    }

    private Vector2 GetPlayerDirection()
    {
        if (_player != null)
        {
            // Return the direction from the boss to the player
            return _player.Position - Position;
        }
        return Vector2.Zero;
    }

    private bool IsPlayerInRange()
    {
        // Check if the player is within attack range
        if (_player != null && Position.DistanceTo(_player.Position) < AttackRange)
        {
            return true;
        }
        return false;
    }
	private void _on_BossAttackArea_body_entered(Node body)
{
    if (body is PlayerController player)
    {
        GD.Print("Player entered attack area, dealing damage!");
        player.ChangeHealth(-100);  // Deal damage
    }
}

}
