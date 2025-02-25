using Godot;
using System;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        // Initialisation code here
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_fullscreen_toggle"))
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen
                ? DisplayServer.WindowMode.Windowed
                : DisplayServer.WindowMode.Fullscreen);
        }
    }

    private void OnStartPressed()
    {
        GD.Print("Start button pressed");
        // Add code to switch scenes or start the game
    }

	private void OnExitGamePressed()
    {
		GetTree().Quit();
    }
}
