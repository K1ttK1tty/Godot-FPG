using Godot;

public partial class Mp5 : Node3D, IWeapon
{
    private int _AmmunitionInMagazine = 30;
    public string WeaponName => "Mp5";
    public string WeaponType => "Range";
    public float Damage => 13;
    public bool IsSoundLoopable => true;
    public string AnimationName => "Ak47";
    public string SoundName => "Ak47";
    public float IntervalBetweenShots => 0.096f;
    public float WaitTimeToGetInHand => 0.8f;
    public float ReloadTime => 1.7f;
    public int AmmunitionInMagazine => _AmmunitionInMagazine;
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
