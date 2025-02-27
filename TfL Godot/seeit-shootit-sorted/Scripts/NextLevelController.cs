using Godot;
using System;

public partial class NextLevelController : Area2D
{
    public override void _Ready()
    {
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

        if (body is PlayerController)
        {
            GetTree().ChangeSceneToFile("res://Scenes/level2Scene.tscn");
        }
    }
}