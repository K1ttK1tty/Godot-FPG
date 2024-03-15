using System.Linq;
using Godot;
public class RifleController : IWeaponTypeController
{
    private readonly static PackedScene Ak47Scene = GD.Load<PackedScene>("res://Weapons/Rifle/AK47/Ak47.tscn");
    private readonly static PackedScene Mp5 = GD.Load<PackedScene>("res://Weapons/Rifle/Mp5/Mp5.tscn");
    private readonly string[] _WeaponNames = { "Ak47", "Mp5" };
    private static System.Collections.Generic.Dictionary<string, IWeapon> Weapons = new(){
        { "Ak47", Ak47Scene.Instantiate<IWeapon>() },
        { "Mp5", Mp5.Instantiate<IWeapon>() },
    };
    private static readonly System.Collections.Generic.Dictionary<string, PackedScene> Scenes = new(){
        { "Ak47", Ak47Scene },
        { "Mp5", Mp5 },
    };
    private static IWeapon _CurrentWeapon = Weapons["Ak47"];
    private static string _CurrentWeaponName = "Ak47";
    private int _AmmunitionQuantity = 300;
    private PackedScene _CurrentScene = Scenes[_CurrentWeaponName];

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
    private int _AmmunitionInMagazine = _CurrentWeapon.AmmunitionInMagazine;
    public int AmmunitionInMagazine => _CurrentWeapon.AmmunitionInMagazine;
    public int AmmunitionQuantity => _AmmunitionQuantity;
    public bool WeaponInHand => false;
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

        GD.Print("CHAGEWEAPON");

        _CurrentWeapon = (IWeapon)Scenes[weapon.WeaponName].Instantiate();
        _CurrentWeaponName = weapon.WeaponName;
        _CurrentScene = Scenes[weapon.WeaponName];
        _AmmunitionInMagazine = weapon.AmmunitionInMagazine;
    }
    public PackedScene GetScene()
    {
        return _CurrentScene;
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