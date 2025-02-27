using Godot;
using System;

public partial class SpikeTrapController : Node
{
    public override void _Ready()
    {
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        GD.Print("Body: " + body + " has entered");
        
        if (body is PlayerController player)
        {
            player.ChangeHealth(-100);
        }
    }
}
