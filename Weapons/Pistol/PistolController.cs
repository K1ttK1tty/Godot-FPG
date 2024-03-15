using System.Linq;
using Godot;
public class PistolController : IWeaponTypeController
{
    private readonly static PackedScene M1911Scene = GD.Load<PackedScene>("res://Weapons/Pistol/M_1911/M_1911.tscn");
    private readonly string[] _WeaponNames = { _CurrentWeapon.WeaponName };
    private static System.Collections.Generic.Dictionary<string, IWeapon> Weapons = new(){
        { "M1911", M1911Scene.Instantiate<IWeapon>() },
    };
    private static readonly System.Collections.Generic.Dictionary<string, PackedScene> Scenes = new(){
        { "M1911", M1911Scene },
    };
    private static IWeapon _CurrentWeapon = Weapons["M1911"];
    private static string _CurrentWeaponName = "M1911";
    private int _AmmunitionQuantity = 70;
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
    public int AmmunitionInMagazine => CurrentWeapon.AmmunitionInMagazine;
    public int AmmunitionQuantity => _AmmunitionQuantity;
    public bool WeaponInHand => false;
    private PackedScene _CurrentScene = Scenes[_CurrentWeaponName];
    public void Reload()
    {
        _AmmunitionQuantity -= AmmunitionInMagazine - CurrentWeapon.AmmunitionInMagazine;
        CurrentWeapon.Reload();
    }
    public void PickUpAmmunition(int value)
    {
        _AmmunitionQuantity += value;
    }
    public void ChangeWeapon(IWeapon weapon)
    {
        if (!_WeaponNames.Contains(weapon.WeaponName)) return ;

        GD.Print(weapon);

        _CurrentWeapon = weapon;
        _CurrentWeaponName = weapon.WeaponName;
        _CurrentScene = Scenes[weapon.WeaponName];
        // _AmmunitionInMagazine = weapon.AmmunitionInMagazine;
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