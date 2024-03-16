using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private static float _Speed = 5.0f;
    private float _SitDownSpeed = _Speed * 0.4f;
    private float _SprintSpeed = _Speed * 1.7f;
    private float _JumpVelocity = 5.2f;
    private readonly static PackedScene BulletScene = GD.Load<PackedScene>("res://Weapons/Bullet/NormalBullet/NormalBullet.tscn");
    private readonly static PackedScene EmptyRay = GD.Load<PackedScene>("res://Empty_ray.tscn");
    private const float _MouseSensitivity = 0.002f;
    private float _Gravity = 18;
    private bool _IsChangeWeaponStage = false;
    private bool _IsReloading = false;
    private bool _IsTeleportTimercooldown = false;
    private bool _IsTeleportReady = false;
    private bool _TimerChargeStage = false;
    private WeaponController Weapon;
    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Fire();
        RayCollidesWithWeapon();
        Teleport();
    }
    public override void _Ready()
    {
        Weapon = new WeaponController();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Node3D newWeapon = Weapon.GetInstantiatedNode();
        newWeapon.ProcessMode = ProcessModeEnum.Disabled;

        // GetNode<Node3D>("RotationHelper/Weapon").AddChild(newWeapon);
    }
    private void OnChangeWeaponTimerTimeout()
    {
        _IsChangeWeaponStage = false;
    }

    private void _SelectWeapon()
    {
        if (_IsChangeWeaponStage) return;
        if (Input.IsActionJustPressed("SelectMeele"))
        {
            Weapon.SelectWeapon("MeleeWeaponController");
            _SelectWeaponLogic();
        }
        else if (Input.IsActionJustPressed("SelectPistol"))
        {
            Weapon.SelectWeapon("PistolController");
            _SelectWeaponLogic();
        }
        else if (Input.IsActionJustPressed("SelectRifle"))
        {
            Weapon.SelectWeapon("RifleController");
            _SelectWeaponLogic();
        }
    }
    private void _SelectWeaponLogic()
    {
        Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        weaponContainer.GetChild(0).QueueFree();

        Node3D newWeapon = Weapon.GetInstantiatedNode();
        newWeapon.ProcessMode = ProcessModeEnum.Disabled;
        weaponContainer.AddChild(newWeapon);
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
    private void Teleport()
    {
        if (_IsTeleportTimercooldown) return;
        if (_IsTeleportReady && Input.IsActionJustPressed("AbortTeleport")) AbortTeleport();
        if (Input.IsActionJustReleased("Teleport") && _TimerChargeStage && !_IsTeleportReady) AbortTeleport();
        if (_IsTeleportReady && Input.IsActionJustReleased("Teleport"))
        {
            GD.Print("Teleport");
            Node3D rotationHelper = GetNode<Node3D>($"RotationHelper");
            Empty_ray ray = EmptyRay.Instantiate<Empty_ray>();
            ray.Rotation = rotationHelper.Rotation;

            ray.Position = new Vector3(GetPositionDelta().X, GetPositionDelta().Y, GetPositionDelta().Z);
            ray.Collide += OnCollide;
            AddChild(ray);

            AbortTeleport();
        }
        if (Input.IsActionPressed("Teleport") && !_TimerChargeStage)
        {
            Timer TeleportTimer = GetNode<Timer>("IsTeleportReadyTimer");
            TeleportTimer.WaitTime = 1;
            TeleportTimer.Start();
            _TimerChargeStage = true;
            GD.Print("Preparation for teleport");
        }
    }

    private void AbortTeleport()
    {
        _IsTeleportTimercooldown = true;
        Timer timer = GetNode<Timer>("TeleportCooldownTimer");
        timer.WaitTime = 1;
        timer.Start();
        GetNode<Timer>("IsTeleportReadyTimer").Stop();
        _IsTeleportReady = false;
        _TimerChargeStage = false;
        GD.Print("Teleport aborted");
        return;
    }

    private void OnCollide(Vector3 ray)
    {
        float zCoord;
        float yCoord;
        float xCoord;
        if (ray.Z > 0)
        {
            zCoord = ray.Z - 0.25f;
        }
        else
        {
            zCoord = ray.Z + 0.25f;
        }

        if (ray.Y > 0)
        {
            yCoord = ray.Y - 0.7f;
        }
        else
        {
            yCoord = ray.Y + 0.7f;
        }

        if (ray.X > 0)
        {
            xCoord = ray.X;
        }
        else
        {
            xCoord = ray.X;
        }

        Position = new Vector3(xCoord, yCoord, zCoord);
        Timer timer = GetNode<Timer>("TeleportCooldownTimer");
        timer.WaitTime = 1;
        // timer.Start();
        _IsTeleportTimercooldown = true;
        GD.Print("Teleported");
    }

    private void OnTeleportCooldownTimer()
    {
        _IsTeleportTimercooldown = false;
        _IsTeleportReady = false;
    }
    private void OnTeleportIsReadyTimer()
    {
        _IsTeleportReady = true;
        GD.Print("Teleport Ready");
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
            GD.Print(Weapon.AmmunitionInMagazine + " " + Weapon.WeaponName);
            Node3D weaponWrapperNode = GetNode<Node3D>($"RotationHelper/Weapon");
            IWeapon weaponNode = (IWeapon)weaponWrapperNode.GetChild(0);
            weaponNode.PlayShootSound();


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
        if (!Input.IsActionPressed("fire"))
        {
            if (Weapon.IsSoundLoopable) GetNode<Timer>("ShootTimer").Stop();
        }
    }
    private void GenerateBullet()
    {
        if (Weapon.WeaponType == "Range")
        {
            Node3D gun = (Node3D)GetNode<Node3D>($"RotationHelper/Weapon").GetChild(0);
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
        Node3D weaponWrapperNode = GetNode<Node3D>($"RotationHelper/Weapon");
        IWeapon weaponNode = (IWeapon)weaponWrapperNode.GetChild(0);
        weaponNode.PlayShootSound();
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
    private void RayCollidesWithWeapon()
    {
        RayCast3D Ray = GetNode<RayCast3D>("RotationHelper/RayCast3D");
        if (Ray.IsColliding())
        {
            if (Ray.GetCollider() is IWeapon weapon)
            {
                weapon.ShowWeaponLabel();
                _ChangeWeapon(weapon);
            }
        }
    }
    private void _ChangeWeapon(IWeapon WeaponName)
    {
        if (Input.IsActionJustPressed("Interact"))
        {
            Weapon.ChangeWeapon(WeaponName);
            _SelectWeaponLogic();
        }
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
            _SelectWeapon();
            InputEventKey keyCode = @event as InputEventKey;
            if (keyCode.Keycode.ToString() == "R")
            {
                if (_IsReloading) return;
                Reloading();
            }
        }
    }
}