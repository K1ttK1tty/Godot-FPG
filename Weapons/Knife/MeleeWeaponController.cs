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
    private PackedScene _CurrentScene = Scenes[_CurrentWeaponName];

    private static IWeapon _CurrentWeapon = Weapons["M9"];
    private static string _CurrentWeaponName = "M9";
    private int _AmmunitionQuantity = 0; // ????????????????????????????????
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
    public bool WeaponInHand => false;
    public void Reload() // ????????????????????????????????
    {
    }
    public void PickUpAmmunition(int value) { }
    public bool ChangeWeapon(IWeapon weapon)
    {
        if (!_WeaponNames.Contains(weapon.WeaponName)) return false;

        GD.Print(weapon);

        _CurrentWeapon = weapon;
        _CurrentWeaponName = weapon.WeaponName;
        _CurrentScene = Scenes[weapon.WeaponName];
        // _CurrentWeapon = Weapons[weapon];
        // _CurrentWeaponName = weapon;
        // _CurrentScene = Scenes[weapon];
        return true;
    }
    public PackedScene GetScene()
    {
        return Scenes[_CurrentWeaponName];
    }
    public Node3D GetInstantiatedNode()
    {
        return GetScene().Instantiate<Node3D>();
    }
    public void Shoot()
    {
        _CurrentWeapon.Shoot();
    }
    public void PlayShootSound()
    {
        _CurrentWeapon.PlayShootSound();
    }
    public void StopShootSound()
    {
        _CurrentWeapon.StopShootSound();
    }
    public void ShowWeaponLabel()
    {

    }
}
