using Godot;
public partial class Ak47 : Node3D, IWeapon

{
    [Export] const float damage = 10;
    public string WeaponName => "Ak47";
    public string WeaponType => "Range";
    public string SoundName => "Ak47";
    public string AnimationName => "Ak47";
    public bool IsSoundLoopable => true;

    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 2f;
    public float Ammunition => 30;


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
