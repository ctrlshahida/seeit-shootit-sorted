using Godot;
using System;

public partial class NextLevelController : Area2D
{
    public override void _Ready()
    {
        // Manually connect the signal in case it wasn't auto-generated
        BodyEntered += OnBodyEntered;
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

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("Player entered portal: " + body.Name);

        // Check if the player entered
        if (body is PlayerController)
        {
            GetTree().ChangeSceneToFile("/Users/batikanozdemir/Projects/SeeItShootItSortedProject/seeit-shootit-sorted/TfL Godot/seeit-shootit-sorted/Scenes/Enemies/EnemiesTest.tscn"); // Change to the correct scene path
        }
    }
}

