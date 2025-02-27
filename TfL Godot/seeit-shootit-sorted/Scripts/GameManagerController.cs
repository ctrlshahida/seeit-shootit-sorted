using Godot;
using System;

public partial class GameManagerController : Node2D
{
    [Export]
    public Marker2D RespawnPoint;

    public override void _Ready()
    {
        PlayerController playerController = GetNode<PlayerController>("CharacterBody2D");

        if (playerController != null)
        {
            playerController.Connect("DeathEventHandler", new Callable(this, "_on_character_body_2d_death"));
        }
        else
        {
            GD.PrintErr("PlayerController node not found.");
        }
    }

    public void RespawnPlayer()
    {
        PlayerController pc = GetNode<PlayerController>("CharacterBody2D");
        if (pc != null)
        {
            pc.GlobalPosition = RespawnPoint.GlobalPosition;
            pc.RespawnPlayer();
        }
    }

    private void _on_character_body_2d_death()
    {
        RespawnPlayer();
    }
}