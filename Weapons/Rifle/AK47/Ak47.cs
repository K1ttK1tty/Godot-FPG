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
    public int AmmunitionInMagazine => _AmmunitionInMagazine;
    private int _AmmunitionInMagazine = 30;
    public float IntervalBetweenShots => 0.13f;
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
        _AmmunitionInMagazine = 30;
    }
    public override void _Ready()
    {
    }
    public override void _Process(double delta)
    {
    }
    public void ShowWeaponLabel()
    {
        if (WeaponInHand) return;
        GetNode<Label3D>("Label3D").Text = WeaponName;
        GetNode<Label3D>("Label3D").Scale = new Vector3(1, 1, 1);
    }
}
