using Godot;
using System;

public partial class PauseMenu : Node
{
    private Control pausePanel;

    public override void _Ready()
    {
        pausePanel = GetNode<Control>("PausePanel");
        pausePanel.Hide();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
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
        pausePanel.Hide();
        GetTree().Paused = false;
    }

    private void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
    }
}