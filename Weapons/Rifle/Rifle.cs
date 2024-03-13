using System.Linq;
using Godot;

public interface IRifle
{
    public IWeapon CurrentWeapon { get; }
    public void ChangeWeapon(string value);
    public PackedScene GetScene();
    public Node3D GetInstantiatedNode();
}
public class Rifle : IRifle
{
    private readonly static PackedScene Ak47Scene = GD.Load<PackedScene>("res://Weapons/Rifle/AK47/Ak47.tscn");
    private readonly string[] _WeaponNames = { _CurrentWeapon.WeaponName };
    public string[] WeaponNames => _WeaponNames;
    private static System.Collections.Generic.Dictionary<string, IWeapon> Weapons = new(){
        { "Ak47", Ak47Scene.Instantiate<IWeapon>() },
    };
    private static readonly System.Collections.Generic.Dictionary<string, PackedScene> Scenes = new(){
        { "Ak47", Ak47Scene },
    };
    private static System.Collections.Generic.Dictionary<string, int> WeaponAmmunition = new(){
        { "Ak47", Weapons["Ak47"].Ammunition },
    };
    private static IWeapon _CurrentWeapon = Weapons["Ak47"];
    private string _CurrentWeaponName = "Ak47";
    public void ChangeWeapon(string value)
    {
        if (!_WeaponNames.Contains(value)) return;

        GD.Print("There is a weapon!!!!");
        _CurrentWeapon = Weapons[value];
        _CurrentWeaponName = value;
    }
    public PackedScene GetScene()
    {
        return Ak47Scene;
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
    public int Ammunition => WeaponAmmunition[_CurrentWeapon.WeaponName];
    public void Shoot()
    {
        WeaponAmmunition[_CurrentWeapon.AnimationName] -= 1;
        _CurrentWeapon.Shoot();
    }
    public void StopShooting()
    {
        _CurrentWeapon.StopShooting();
    }
}