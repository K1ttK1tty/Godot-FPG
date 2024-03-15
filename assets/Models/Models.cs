using Godot;
interface IWeaponController
{
    public void SelectWeapon(string value);
    public string CurrentControllerName { get; }
    // public void ChangeWeapon(string value);



    public IWeapon CurrentWeapon { get; }
    public PackedScene GetScene();
    public Node3D GetInstantiatedNode();
    public int AmmunitionQuantity { get; }
    public void PickUpAmmunition(int value);





    public string WeaponName { get; }
    public string WeaponType { get; } // melee | range
    public float Damage { get; }
    public bool IsSoundLoopable { get; }
    public string AnimationName { get; }
    public string SoundName { get; }
    public float IntervalBetweenShots { get; }
    public float WaitTimeToGetInHand { get; }
    public float ReloadTime { get; } // if meele i don't need this, change interface
    public int AmmunitionInMagazine { get; } // if meele i don't need this, change interface
    public bool WeaponInHand { get; }
    public void Shoot();
    public void StopShootSound();
    public void Reload();
    public void PlayShootSound();
    public void ShowWeaponLabel();
}

public interface IWeaponTypeController
{
    public IWeapon CurrentWeapon { get; }
    public bool ChangeWeapon(IWeapon value);
    public PackedScene GetScene();
    public Node3D GetInstantiatedNode();
    public int AmmunitionQuantity { get; }
    public void PickUpAmmunition(int value);





    public string WeaponName { get; }
    public string WeaponType { get; } // melee | range
    public float Damage { get; }
    public bool IsSoundLoopable { get; }
    public string AnimationName { get; }
    public string SoundName { get; }
    public float IntervalBetweenShots { get; }
    public float WaitTimeToGetInHand { get; }
    public float ReloadTime { get; } // if meele i don't need this, change interface
    public int AmmunitionInMagazine { get; } // if meele i don't need this, change interface
    public bool WeaponInHand { get; }
    public void Shoot();
    public void StopShootSound();
    public void Reload();
    public void PlayShootSound();
    public void ShowWeaponLabel();
}
public interface IWeapon
{

    public string WeaponName { get; }
    public string WeaponType { get; } // melee | range
    public float Damage { get; }
    public bool IsSoundLoopable { get; }
    public string AnimationName { get; }
    public string SoundName { get; }
    public float IntervalBetweenShots { get; }
    public float WaitTimeToGetInHand { get; }
    public float ReloadTime { get; } // if meele i don't need this, change interface
    public int AmmunitionInMagazine { get; } // if meele i don't need this, change interface
    public bool WeaponInHand { get; }
    public void Shoot();
    public void StopShootSound();
    public void Reload();
    public void PlayShootSound();
    public void ShowWeaponLabel();
}
