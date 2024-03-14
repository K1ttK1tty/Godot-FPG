using System.Linq;
using Godot;
public class MeleeWeaponController : IWeaponTypeController
{
    private readonly static PackedScene M9Knife = GD.Load<PackedScene>("res://Weapons/Knife/M9/M9.tscn");

    private readonly string[] _WeaponNames = { _CurrentWeapon.WeaponName };
    private static System.Collections.Generic.Dictionary<string, IWeapon> Weapons = new(){
        { "M9", M9Knife.Instantiate<IWeapon>() },
    };
    private static readonly System.Collections.Generic.Dictionary<string, PackedScene> Scenes = new(){
        { "M9", M9Knife },
    };
    private static IWeapon _CurrentWeapon = Weapons["M9"];
    private string _CurrentWeaponName = "M9";
    private int _AmmunitionQuantity = 0; // ????????????????????????????????
    public void Reload() // ????????????????????????????????
    {
    }
    public void PickUpAmmunition(int value) { }
    public void ChangeWeapon(string value)
    {
        if (!_WeaponNames.Contains(value)) return;

        GD.Print("There is a weapon!!!!");
        _CurrentWeapon = Weapons[value];
        _CurrentWeaponName = value;
    }
    public PackedScene GetScene()
    {
        return M9Knife;
    }
    public Node3D GetInstantiatedNode()
    {
        return GetScene().Instantiate<Node3D>();
    }

    public IWeapon CurrentWeapon { get => _CurrentWeapon; }
    public string WeaponName => _CurrentWeaponName;
    public string WeaponType => _CurrentWeapon.WeaponType;
    public float Damage => _CurrentWeapon.Damage;
    public bool IsSoundLoopable => _CurrentWeapon.IsSoundLoopable;
    public string AnimationName => _CurrentWeapon.AnimationName;
    public string SoundName => _CurrentWeapon.SoundName;
    public float IntervalBetweenShots => _CurrentWeapon.IntervalBetweenShots;
    public float WaitTimeToGetInHand => _CurrentWeapon.WaitTimeToGetInHand;
    public float ReloadTime => _CurrentWeapon.ReloadTime;
    public int AmmunitionInMagazine => 0;
    public int AmmunitionQuantity => _AmmunitionQuantity; // ????????????????????????????????
    public void Shoot()
    {
        _CurrentWeapon.Shoot();
    }
    public void PlayShootSound(){
        _CurrentWeapon.PlayShootSound();
    }
    public void StopShootSound()
    {
        _CurrentWeapon.StopShootSound();
    }
}
