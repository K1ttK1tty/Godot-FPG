using Godot;

public partial class Knife : Node3D, IWeapon
{

    [Export] const float damage = 4;

    public string WeaponName => "Knife";
    public string WeaponType => "Melee";
    public string SoundName => "Knife";
    public string AnimationName => "Knife";
    public bool IsSoundLoopable => false;
    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 2f; // ???????????????????
    public float Ammunition => 0; // ???????????????????

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
