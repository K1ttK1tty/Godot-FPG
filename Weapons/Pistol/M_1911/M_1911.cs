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
    public bool WeaponInHand => false;
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
        _AmmunitionInMagazine = 7;
    }
    public override void _Ready()
    {
    }
    public override void _Process(double delta)
    {
    }
    public void ShowWeaponLabel()
    {


    }
}
