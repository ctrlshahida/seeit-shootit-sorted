using Godot;
using System;

public partial class NextLevelController : Area2D
{
    public override void _Ready()
    {
        // Manually connect the signal in case it wasn't auto-generated
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("Player entered portal: " + body.Name);

        // Check if the player entered
        if (body is PlayerController)
        {
            GetTree().ChangeSceneToFile("res://level2Scene.tscn"); // Change to the correct scene path
        }
    }
}

