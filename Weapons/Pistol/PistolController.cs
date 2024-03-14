using System.Linq;
using Godot;
public class PistolController : IWeaponTypeController
{
    private readonly static PackedScene M1911Scene = GD.Load<PackedScene>("res://Weapons/Pistol/M_1911/M_1911.tscn");
    private readonly string[] _WeaponNames = { _CurrentWeapon.WeaponName };
    // public string[] WeaponNames => _WeaponNames;
    private static System.Collections.Generic.Dictionary<string, IWeapon> Weapons = new(){
        { "M1911", M1911Scene.Instantiate<IWeapon>() },
    };
    private static readonly System.Collections.Generic.Dictionary<string, PackedScene> Scenes = new(){
        { "M1911", M1911Scene },
    };
    private static IWeapon _CurrentWeapon = Weapons["M1911"];
    private string _CurrentWeaponName = "M1911";
    private int _AmmunitionQuantity = 70;
    public void Reload()
    {
        _AmmunitionQuantity -= AmmunitionInMagazine - CurrentWeapon.AmmunitionInMagazine;
        CurrentWeapon.Reload();
    }
    public void PickUpAmmunition(int value)
    {
        _AmmunitionQuantity += value;
    }
    public void ChangeWeapon(string value)
    {
        if (!_WeaponNames.Contains(value)) return;

        GD.Print("There is a weapon!!!!");
        _CurrentWeapon = Weapons[value];
        _CurrentWeaponName = value;
    }
    public PackedScene GetScene()
    {
        return M1911Scene;
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
    public int AmmunitionInMagazine => CurrentWeapon.AmmunitionInMagazine;
    public int AmmunitionQuantity => _AmmunitionQuantity;
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
}