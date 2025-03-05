using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class NextLevelBossController : Area2D
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
        if (body is PlayerController pc)
        {
            GlobalState.Instance.PlayerHealth = pc.MaxHealth;

            GetTree().ChangeSceneToFile("Scenes/Level3Scene/level_3.tscn"); // Change to the correct scene path
        }
    }
}
