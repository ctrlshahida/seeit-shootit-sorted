using Godot;
using System;

public partial class Level2GameController : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Node gunContainer;
	private PlayerController player;
    private FareDodger_AS enemy;

	[Export] public Marker2D RespawnPoint;
	public override void _Ready()
	{
		gunContainer = GetNode<Node>("GunContainer");
        player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player != null)
        {
            GD.Print("Signal connected to player");
            player.Connect(PlayerController.SignalName.GunShot, new Callable(this, nameof(OnPlayerGunShot)));
        }
        else
        {
            GD.PrintErr("Player not found in group.");
        }



        enemy = GetNode<FareDodger_AS>("EnemyNode"); // Assuming enemy is a direct child of GameManager
        if (enemy != null)
        {
            GD.Print("Connecting death signal from enemy");
            enemy.Connect("Death", new Callable(this, "_on_enemy_death"));
        }
        else
        {
            GD.PrintErr("Enemy node not found!");
        }
	}

    // Method triggered by the death signal from the enemy
    private void _on_enemy_death()
    {
        GD.Print("Enemy has died! Triggering respawn logic or other actions.");
        RespawnPlayer(); // Example: Respawn player or do something else
    }


	public void OnPlayerGunShot(PackedScene gunScene, Vector2 location, Vector2 direction)
    {
        if (gunScene == null)
        {
            GD.PrintErr("Gun scene is null!");
            return;
        }
        // Instantiate the gun
        Area2D gun = gunScene.Instantiate<Area2D>();
        gun.GlobalPosition = location;
        // Check if the gun has the "set_direction" method and call it
        if (gun.HasMethod("set_direction"))
        {
            gun.Call("set_direction", direction);
        }
        else
        {
            GD.PrintErr("Gun does not have set_direction method!");
        }
        // Add gun to the container
        gunContainer.AddChild(gun);
    }
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RespawnPlayer(){
        GD.Print("Level 2 Respawn Point:", RespawnPoint.GlobalPosition);
		PlayerController pc = GetNode<PlayerController>("CharacterBody2D");
		pc.GlobalPosition = RespawnPoint.GlobalPosition;
		pc.RespawnPlayer();
	}

	private void _on_character_body_2d_death(){
		RespawnPlayer();
	}
}

