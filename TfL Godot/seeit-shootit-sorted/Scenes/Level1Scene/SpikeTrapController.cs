using Godot;
using System;

public partial class SpikeTrapController : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	private void _on_area_2d_body_entered(Node2D body){
		GD.Print("Body: " + body + " has entered");
		if (body is PlayerController){
			PlayerController pc = body as PlayerController;
			pc.ChangeHealth(-100);
		}
	}


	private void _on_area_2d_body_entered_FD(Node2D bodyAS)
    {
		GD.Print("Enemy: " + bodyAS + " has entered");
		if (bodyAS is FareDodger_AS)
		{
			FareDodger_AS fd = bodyAS as FareDodger_AS;
			fd.ChangeHealth(-100);
		}
	}
}