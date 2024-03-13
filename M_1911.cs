using Godot;
public partial class M_1911 : Node3D, IWeapon
{
    [Export] const float damage = 5;

    public string WeaponName => "M1911";
    public string WeaponType => "Range";
    public string AnimationName => "M1911";
    public bool IsSoundLoopable => false;

    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 2f;
    public float Ammunition => 7;

    public string SoundName => "M1911";

    public float IntervalBetweenShots => 0.12f;
    public void Shoot()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
    public void StopShooting()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
