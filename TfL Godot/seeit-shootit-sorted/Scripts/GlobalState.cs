using Godot;
using System;

public partial class GlobalState : Node
{
    public static GlobalState Instance { get; private set; }

    public int PlayerHealth { get; set; } = 100; // Default to 100
    public PlayerController Player { get; set; } // Store player reference

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            SetProcess(false); // No need to process
        }
        else
        {
            QueueFree(); // Avoid duplicates
        }
    }
}
