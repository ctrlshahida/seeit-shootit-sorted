using Godot;
using System;

public partial class Enemy01 : RigidBody2D
{
	Sprite2D sprite;
	RayCast2D bottomLeft;
	RayCast2D bottomRight;

	private Vector2 velocity;
	private int gravity = 200;
	private int speed = 30;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite");
		bottomLeft = GetNode<RayCast2D>("LeftRaycast");
		bottomRight = GetNode<RayCast2D>("RightRaycast");
		velocity.X = speed;
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(double delta)
    // {
    // }
public void _PhysicsProcess(float delta)
{
    //gravity
	velocity.Y += gravity * delta;
    if(velocity.Y > gravity)
    {
        velocity.Y = gravity;
    }

    // Apply gravity and velocity to the RigidBody2D
    ApplyCentralForce(Vector2.Up * -gravity);
    ApplyCentralForce(velocity);
}


    // public override void _on_area_2d_body_entered(Area2D body)
    // {
    //     if (body.name == "CharacterBody2D")
    //     {
    //         var y_delta = Position.Y - body.Position.Y;
    //         if (y_delta > 30)
    //         {
    //             Console.WriteLine("Destroy enemy");
	// 			QueueFree();
	// 			body.jump();
    //         }
    //         else
    //         {
    //             Console.WriteLine("Decrease player health ");
    //         }
    //     }
    // }
}
