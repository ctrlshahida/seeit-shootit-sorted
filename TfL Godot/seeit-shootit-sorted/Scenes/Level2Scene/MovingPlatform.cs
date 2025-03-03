using Godot;

public partial class MovingPlatform : PathFollow2D
{
    [Export] public float Speed = 100.0f;  // Platform speed
    [Export] public bool Loop = true;  // If false, platform will reverse at the ends

    private int _direction = 1;  // 1 = forward, -1 = backward

    public override void _Process(double delta)
    {
        Progress += (float)(Speed * _direction * delta);

        if (!Loop)
        {
            if (ProgressRatio >= 1.0f) // Reached the end
            {
                ProgressRatio = 1.0f;
                _direction = -1;
            }
            else if (ProgressRatio <= 0.0f) // Reached the start
            {
                ProgressRatio = 0.0f;
                _direction = 1;
            }
        }
    }
}
