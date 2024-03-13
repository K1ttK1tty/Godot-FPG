using Godot;


public partial class Bullet : Node3D
{
    [Export] const float speed = 55f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += Transform.Basis * new Vector3(0, 0, -speed) * (float)delta;
    }
    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
