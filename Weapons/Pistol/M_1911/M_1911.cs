using Godot;
public partial class M_1911 : Node3D, IWeapon
{
    public float Damage => 8;
    public string WeaponName => "M1911";
    public string WeaponType => "Range";
    public string AnimationName => "M1911";
    public bool IsSoundLoopable => false;
    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 1.7f;
    public int AmmunitionInMagazine => _AmmunitionInMagazine;
    private int _AmmunitionInMagazine = 7;
    public string SoundName => "M1911";
    public float IntervalBetweenShots => 0.12f;
    public void Shoot()
    {
        _AmmunitionInMagazine -= 1;
    }
    public void PlayShootSound()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
    public void StopShootSound()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
    }
    public void Reload()
    {
        GD.Print("REload");
        _AmmunitionInMagazine = 7;
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
