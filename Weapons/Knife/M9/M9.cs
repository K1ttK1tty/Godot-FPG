using Godot;
public partial class M9 : Node3D, IWeapon
{
    public float Damage => 7;
    public string WeaponName => "Knife";
    public string WeaponType => "Melee";
    public string SoundName => "Knife";
    public string AnimationName => "Knife";
    public bool IsSoundLoopable => false;
    public float WaitTimeToGetInHand => 1f;
    public float ReloadTime => 0f; // ?????????????????
    public int AmmunitionInMagazine => 0; // ???????????????????

    public float IntervalBetweenShots => 0.12f;
    public bool WeaponInHand => false;

    public void Shoot()
    {
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
