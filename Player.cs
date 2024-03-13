using System;
using Godot;
interface IWeapon
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
    public float Ammunition { get; } // if meele i don't need this, change interface
    public void Shoot();
    public void StopShooting();
}

public partial class Player : CharacterBody3D
{
    private static float _Speed = 5.0f;
    private float _SitDownSpeed = _Speed * 0.4f;
    private float _SprintSpeed = _Speed * 1.7f;
    private float _JumpVelocity = 5.2f;
    private readonly static PackedScene BulletScene = GD.Load<PackedScene>("res://Weapons/Bullet/NormalBullet/NormalBullet.tscn");
    private readonly static PackedScene WeaponKnifeScene = GD.Load<PackedScene>("res://Weapons/Knife/M9/M9.tscn");
    private readonly static PackedScene WeaponPistolScene = GD.Load<PackedScene>("res://Weapons/Pistol/M_1911/M_1911.tscn");
    private readonly static PackedScene WeaponRifleScene = GD.Load<PackedScene>("res://Weapons/Rifle/AK47/Ak47.tscn");
    private const float _MouseSensitivity = 0.002f;
    private float _Gravity = 18;
    private bool _IsChangeWeaponStage = false;
    private bool _IsReloading = false;
    private static string _CurrentWeaponName = "Rifle";
    private static readonly PackedScene[] WeaponScenes = { WeaponKnifeScene, WeaponPistolScene, WeaponRifleScene };
    // private readonly PackedScene _CurrentWeaponScene = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]];
    private readonly System.Collections.Generic.Dictionary<string, int> WeaponsIndexes = new(){
        { "Knife", 0 },
        { "Pistol", 1 },
        { "Rifle", 2 }
    };
    private readonly System.Collections.Generic.Dictionary<string, string> WeaponNames = new(){
        {"Knife","Knife"},
        {"Pistol","M1911"},
        {"Rifle","Ak47"}
    };
    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Fire();
    }
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
        GetNode<Node3D>("RotationHelper/Weapon").AddChild(weapon);
    }
    private void OnChangeWeaponTimerTimeout()
    {
        _IsChangeWeaponStage = false;
    }
    private void ChangeWeapon()
    {
        if (!_IsChangeWeaponStage)
        {
            if (Input.IsActionJustPressed("SelectMeele") && _CurrentWeaponName != "Knife")
            {
                SelectWeapon("Knife");
            }
            else if (Input.IsActionJustPressed("SelectPistol") && _CurrentWeaponName != "Pistol")
            {
                SelectWeapon("Pistol");
            }
            else if (Input.IsActionJustPressed("SelectRifle") && _CurrentWeaponName != "Rifle")
            {
                SelectWeapon("Rifle");
            }
        }
    }
    private void SelectWeapon(string newWeapon)
    {
        _CurrentWeaponName = newWeapon;
        Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
        Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        weaponContainer.GetChild(0).QueueFree();
        weaponContainer.AddChild(weapon);
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
            IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            if (currentWeapon.Ammunition == 0)
            {
                Reloading();
                return;
            }
            currentWeapon.Shoot();
            GD.Print(currentWeapon.Ammunition);
            if (currentWeapon.IsSoundLoopable)
            {
                Timer shootTimer = GetNode<Timer>("ShootTimer");
                shootTimer.WaitTime = currentWeapon.IntervalBetweenShots;
                shootTimer.Start();
            }
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play(currentWeapon.AnimationName);
            GenerateBullet();
        }

        bool isWeaponExistInScene = GetNode<Node3D>("RotationHelper/Weapon").GetChild(0) != null;
        if (!Input.IsActionPressed("fire") && isWeaponExistInScene)
        {
            IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            if (currentWeapon.IsSoundLoopable) GetNode<Timer>("ShootTimer").Stop();
        }
    }
    private void GenerateBullet()
    {
        if (GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}").WeaponType == "Range")
        {
            Node3D gun = GetNode<Node3D>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            Bullet bullet = BulletScene.Instantiate<Bullet>();
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
        IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetNode<AnimationPlayer>("AnimationPlayer").Play(currentWeapon.AnimationName);
        GetNode<IWeapon>($"RotationHelper/Weapon/{currentWeapon.WeaponName}").Shoot();
        GenerateBullet();
    }
    private void Reloading()
    {
        if (_CurrentWeaponName != "Knife")
        {
            _IsReloading = true;
            IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            Timer ReloadingTimer = GetNode<Timer>("ReloadingTimer");
            ReloadingTimer.WaitTime = currentWeapon.ReloadTime;
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
            if (keyCode.Keycode.ToString() == "R") Reloading();
        }
    }
}