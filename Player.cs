using System;
using Godot;
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
    public void Shoot();
    public void StopShootSound();
    public void Reload();
    public void PlayShootSound();
}
public partial class Player : CharacterBody3D
{
    private static float _Speed = 5.0f;
    private float _SitDownSpeed = _Speed * 0.4f;
    private float _SprintSpeed = _Speed * 1.7f;
    private float _JumpVelocity = 5.2f;
    private readonly static PackedScene BulletScene = GD.Load<PackedScene>("res://Weapons/Bullet/NormalBullet/NormalBullet.tscn");
    // private readonly static PackedScene WeaponKnifeScene = GD.Load<PackedScene>("res://Weapons/Knife/M9/M9.tscn");
    // private readonly static PackedScene WeaponPistolScene = GD.Load<PackedScene>("res://Weapons/Pistol/M_1911/M_1911.tscn");
    // private readonly static PackedScene WeaponRifleScene = GD.Load<PackedScene>("res://Weapons/Rifle/AK47/Ak47.tscn");
    private const float _MouseSensitivity = 0.002f;
    private float _Gravity = 18;
    private bool _IsChangeWeaponStage = false;
    private bool _IsReloading = false;
    private WeaponController Weapon;
    // private static string _CurrentWeaponName = "Rifle";
    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Fire();
    }
    public override void _Ready()
    {
        Weapon = new WeaponController();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GetNode<Node3D>("RotationHelper/Weapon").AddChild(Weapon.GetInstantiatedNode());
    }
    private void OnChangeWeaponTimerTimeout()
    {
        _IsChangeWeaponStage = false;
    }
    private void ChangeWeapon()
    {
        if (!_IsChangeWeaponStage)
        {
            if (Input.IsActionJustPressed("SelectMeele") && Weapon.CurrentControllerName != "MeleeWeaponController")
            {
                Weapon.SelectWeapon("MeleeWeaponController");
                _SelectWeapon();

            }
            else if (Input.IsActionJustPressed("SelectPistol") && Weapon.CurrentControllerName != "PistolController")
            {
                Weapon.SelectWeapon("PistolController");
                _SelectWeapon();
            }
            else if (Input.IsActionJustPressed("SelectRifle") && Weapon.CurrentControllerName != "RifleController")
            {
                Weapon.SelectWeapon("RifleController");
                _SelectWeapon();
            }
        }
    }
    private void _SelectWeapon()
    {
        Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        weaponContainer.GetChild(0).QueueFree();
        weaponContainer.AddChild(Weapon.GetInstantiatedNode());
        _IsChangeWeaponStage = true;
        GetNode<Timer>("ShootTimer").Stop();
        GetNode<Timer>("ChangeWeaponTimer").Start();
    }
    private void Movement(double delta)
    {
        Vector3 velocity = Velocity;
        if (!IsOnFloor()) velocity.Y -= _Gravity * (float)delta;

        if (Input.IsActionJustPressed("jump") && IsOnFloor()) velocity.Y = _JumpVelocity;

        if (Input.IsActionPressed("sprint") && IsOnFloor()) _Speed = _SprintSpeed;
        else _Speed = 5f;

        if (Input.IsActionPressed("sit_down") && IsOnFloor())
        {
            Scale = new Vector3(1, 0.6f, 1);
            _Speed = _SitDownSpeed;
        }
        else if (!Input.IsActionPressed("sprint"))
        {
            _Speed = 5f;
            Scale = new Vector3(1, 1, 1);
        }
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * _Speed;
            velocity.Z = direction.Z * _Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, _Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _Speed);
        }
        Velocity = velocity;
        MoveAndSlide();
    }
    private void Fire()
    {
        if (_IsReloading)
        {
            GetNode<Timer>("ShootTimer").Stop();
            return;
        }
        if (_IsChangeWeaponStage)
        {
            GetNode<Timer>("ShootTimer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            return;
        }

        if (Input.IsActionJustPressed("fire"))
        {
            if (Weapon.AmmunitionInMagazine == 0 && Weapon.WeaponType == "Range")
            {
                Reloading();
                return;
            }
            Weapon.Shoot();
            GetNode<IWeapon>($"RotationHelper/Weapon/{Weapon.WeaponName}").PlayShootSound();

            if (Weapon.IsSoundLoopable)
            {
                Timer shootTimer = GetNode<Timer>("ShootTimer");
                shootTimer.WaitTime = Weapon.IntervalBetweenShots;
                shootTimer.Start();
            }
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play(Weapon.AnimationName);
            GenerateBullet();
        }

        // bool isWeaponExistInScene = GetNode<Node3D>("RotationHelper/Weapon").GetChild(0) != null; // ????????
        // if (!Input.IsActionPressed("fire") && isWeaponExistInScene)
        if (!Input.IsActionPressed("fire"))
        {
            if (Weapon.IsSoundLoopable) GetNode<Timer>("ShootTimer").Stop();
        }
    }
    private void GenerateBullet()
    {
        if (Weapon.WeaponType == "Range")
        {
            Node3D gun = GetNode<Node3D>($"RotationHelper/Weapon/{Weapon.WeaponName}");
            NormalBullet bullet = BulletScene.Instantiate<NormalBullet>();
            bullet.Position = gun.Position;
            bullet.Transform = GetNode<Node3D>("RotationHelper").Transform;
            AddChild(bullet);
        }
    }
    private void OnShootTimerTimeout()
    {

        if (_IsChangeWeaponStage)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            return;
        }
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetNode<AnimationPlayer>("AnimationPlayer").Play(Weapon.AnimationName);
        if (Weapon.AmmunitionInMagazine == 0 && Weapon.WeaponType == "Range")
        {

            Reloading();
            return;
        }
        Weapon.Shoot();
        GetNode<IWeapon>($"RotationHelper/Weapon/{Weapon.WeaponName}").PlayShootSound();
        GenerateBullet();
    }
    private void Reloading()
    {
        if (Weapon.CurrentControllerName != "MeleeWeaponController")
        {
            _IsReloading = true;
            Weapon.Reload();
            Timer ReloadingTimer = GetNode<Timer>("ReloadingTimer");
            ReloadingTimer.WaitTime = Weapon.ReloadTime;
            ReloadingTimer.Start();
            GD.Print("StartReloading");
        }
    }
    private void OnReloadingTimerTimeout()
    {
        _IsReloading = false;
        GD.Print("EndReloading");
    }
    public override void _Input(InputEvent @event)
    {
        Node3D RotationHelper = GetNode<Node3D>("RotationHelper");
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            RotateY(-mouseEvent.Relative.X * _MouseSensitivity);
            RotationHelper.RotateX(-mouseEvent.Relative.Y * _MouseSensitivity);
            Vector3 cameraRot = RotationHelper.RotationDegrees;
            cameraRot.X = Math.Clamp(cameraRot.X, -70, 70);
            RotationHelper.RotationDegrees = cameraRot;
        }
        if (@event is InputEventKey)
        {
            ChangeWeapon();
            InputEventKey keyCode = @event as InputEventKey;
            if (keyCode.Keycode.ToString() == "R")
            {
                if (_IsReloading) return;
                Reloading();
            }
        }
    }
}