using Godot;

public partial class Empty_ray : Node3D
{
    [Signal]
    public delegate void CollideEventHandler(Vector3 myString);
    [Export] const float speed = 60;
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Start();
    }
    public override void _Process(double delta)
    {
        Position += Transform.Basis * new Vector3(0, 0, -speed) * (float)delta;
        RayCast3D Ray = GetNode<RayCast3D>("RayCast3D");
        if (Ray.IsColliding())
        {
            // GD.Print(Position);
            GD.Print("COLLIDE");
            EmitSignal(SignalName.Collide, GlobalPosition);
            QueueFree();
        }
    }
    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
