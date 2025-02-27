using Godot;
using System;

public partial class PauseMenu : Node
{
    private Control pausePanel;

    public override void _Ready()
    {
        pausePanel = GetNode<Control>("PausePanel");
        pausePanel.Hide();
        GetTree().Paused = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            GD.Print("Pause button pressed");

            if (GetTree().Paused)
            {
                GetTree().Paused = false;
                pausePanel.Hide();
            }
            else
            {
                GetTree().Paused = true;
                pausePanel.Show();
            }
        }
    }

    private void OnResumePressed()
    {
        GD.Print("Resume Button Pressed");
        pausePanel.Hide();
        GetTree().Paused = false;
    }

    private void OnMainMenuPressed()
    {
        GD.Print("Main Menu Button Pressed");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
    }
}