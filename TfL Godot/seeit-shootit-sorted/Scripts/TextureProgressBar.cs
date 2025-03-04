using Godot;
using System;

public partial class TextureProgressBar : Godot.TextureProgressBar
{
[Export] public PlayerController player;

    public override void _Ready()
    {
        base._Ready();

        player = GlobalState.Instance.Player;

        if (player != null)
        {
            player.HealthChanged += UpdateHealthBar;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (player != null)
        {
            Value = (float)(player.CurrentHealth * 100) / player.MaxHealth;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("ui_fullscreen_toggle"))
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen
                ? DisplayServer.WindowMode.Windowed
                : DisplayServer.WindowMode.Fullscreen);
        }
    }
}
