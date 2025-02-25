using Godot;
using System;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        // Initialisation code here
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

    private void OnStartPressed()
    {
        GD.Print("Start button pressed");
    }

	private void OnExitGamePressed()
    {
		GetTree().Quit();
    }
}
