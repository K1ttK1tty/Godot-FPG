using Godot;
public partial class NormalBullet : Node3D
{
    [Export] const float speed = 55f;
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
            GD.Print(Ray.GetCollider());
            QueueFree();
        }
    }
    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
