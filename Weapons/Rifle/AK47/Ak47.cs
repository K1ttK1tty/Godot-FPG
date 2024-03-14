using Godot;
public partial class Ak47 : Node3D, IWeapon

{
    public float Damage => 15;
    public string WeaponName => "Ak47";
    public string WeaponType => "Range";
    public string SoundName => "Ak47";
    public string AnimationName => "Ak47";
    public bool IsSoundLoopable => true;
    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 2.3f;
    public int AmmunitionInMagazine => 30;
    // AmmunitionInMagazine in the magazine 
    public float IntervalBetweenShots => 0.12f;


    // public float IWeapon.AmmunitionInMagazine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
