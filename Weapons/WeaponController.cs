using System.Linq;
using Godot;
interface IWeaponController : IWeaponTypeController
{
    public void SelectWeapon(string value);
    public string CurrentControllerName { get; }
}
public class WeaponController : IWeaponController
{
    private static RifleController _RifleController = new RifleController();
    private static PistolController _PistolController = new PistolController();
    private static MeleeWeaponController _MeleeWeaponController = new MeleeWeaponController();

    private static System.Collections.Generic.Dictionary<string, IWeaponTypeController> Controllers = new(){
        { "RifleController", _RifleController },
        { "PistolController", _PistolController },
        { "MeleeWeaponController", _MeleeWeaponController },
    };

    private static string _CurrentControllerName = "PistolController";
    private static IWeaponTypeController _CurrentController = Controllers[_CurrentControllerName];
    private string _CurrentWeaponName = _CurrentController.WeaponName;
    private IWeapon _CurrentWeapon = _CurrentController.CurrentWeapon;
    // private readonly string[] _WeaponNames = { _CurrentController.WeaponName };

    public void Reload()
    {
        _CurrentController.Reload();
    }
    public PackedScene GetScene()
    {
        return _CurrentController.GetScene();
    }
    public Node3D GetInstantiatedNode()
    {
        return _CurrentController.GetInstantiatedNode();
    }
    public void PickUpAmmunition(int value)
    {
        _CurrentController.PickUpAmmunition(value);
    }
    public void SelectWeapon(string value)
    {
        if (Controllers[value] == null)
        {
            throw new System.Exception("Weapon doesn't exeist");
        }
        GD.Print("Select");
        _CurrentControllerName = value;
        _CurrentController = Controllers[value];
        _CurrentWeapon = Controllers[value].CurrentWeapon;
        _CurrentWeaponName = Controllers[value].WeaponName;
    }
    public void ChangeWeapon(string value)
    {
        _CurrentController.ChangeWeapon(value);
    }

    public string CurrentControllerName => _CurrentControllerName;
    public IWeapon CurrentWeapon => _CurrentWeapon;
    public string WeaponName => _CurrentWeaponName;
    public string WeaponType => _CurrentWeapon.WeaponType;
    public float Damage => _CurrentWeapon.Damage;
    public bool IsSoundLoopable => _CurrentWeapon.IsSoundLoopable;
    public string AnimationName => _CurrentWeapon.AnimationName;
    public string SoundName => _CurrentWeapon.SoundName;
    public float IntervalBetweenShots => _CurrentWeapon.IntervalBetweenShots;
    public float WaitTimeToGetInHand => _CurrentWeapon.WaitTimeToGetInHand;
    public float ReloadTime => _CurrentWeapon.ReloadTime;
    public int AmmunitionInMagazine => _CurrentController.AmmunitionInMagazine;
    public int AmmunitionQuantity => _CurrentController.AmmunitionQuantity;
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