using Godot;
using System;

public partial class GameManagerController : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public Marker2D RespawnPoint;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RespawnPlayer(){
		PlayerController pc = GetNode<PlayerController>("CharacterBody2D");
		pc.GlobalPosition = RespawnPoint.GlobalPosition;
		pc.RespawnPlayer();
	}

	private void _on_character_body_2d_death(){
		RespawnPlayer();
	}
}
