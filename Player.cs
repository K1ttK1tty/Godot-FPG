using System;
using Godot;
interface IWeapon
{
    public string WeaponName { get; }
    public string WeaponType { get; } // melee | range
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
    private static float Speed = 5.0f;
    private float _sitDownSpeed = Speed * 0.4f;
    private float _sprintSpeed = Speed * 1.7f;
    public const float JumpVelocity = 5.2f;
    private readonly static PackedScene BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
    private readonly static PackedScene WeaponKnifeScene = GD.Load<PackedScene>("res://Knife.tscn");
    private readonly static PackedScene WeaponPistolScene = GD.Load<PackedScene>("res://M_1911.tscn");
    private readonly static PackedScene WeaponRifleScene = GD.Load<PackedScene>("res://Ak47.tscn");
    public const float MouseSensitivity = 0.002f;
    public float gravity = 18;
    private bool ChangeWeaponStage = false;

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
    private void OnChangeWeaponTimer()
    {
        ChangeWeaponStage = false;
        GD.Print("OnChangeWeaponTimer");
    }

    private void ChangeWeapon(string keyCode)
    {
        // if (
        //     Input.IsActionJustPressed("SelectMeele") ||
        //     Input.IsActionJustPressed("SelectPistol") ||
        //     Input.IsActionJustPressed("SelectRifle")
        //     )
        // {
        //     switch (keyCode)
        //     {
        //         case "Key1":
        //             _CurrentWeaponName = "Knife";
        //             Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
        //             Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        //             weaponContainer.GetChild(0).QueueFree();
        //             weaponContainer.AddChild(weapon);
        //             break;
        //         case "Key2":
        //             _CurrentWeaponName = "Pistol";
        //             weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
        //             weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        //             weaponContainer.GetChild(0).QueueFree();
        //             weaponContainer.AddChild(weapon); 
        //             break;
        //         case "Key3":
        //             _CurrentWeaponName = "Rifle";
        //             weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
        //             weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
        //             weaponContainer.GetChild(0).QueueFree();
        //             weaponContainer.AddChild(weapon); 
        //             break;
        //         default:
        //             break;
        //     }
        // }
        if (!ChangeWeaponStage)
        {


            if (Input.IsActionJustPressed("SelectMeele"))
            {
                _CurrentWeaponName = "Knife";
                Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
                Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
                weaponContainer.GetChild(0).QueueFree();
                GD.Print(weaponContainer.GetChildren().Count);
                weaponContainer.AddChild(weapon);

                ChangeWeaponStage = true;
                GetNode<Timer>("ChangeWeaponTimer").Start();
            }
            else if (Input.IsActionJustPressed("SelectPistol"))
            {
                _CurrentWeaponName = "Pistol";
                Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
                Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
                weaponContainer.GetChild(0).QueueFree();
                weaponContainer.AddChild(weapon);

                ChangeWeaponStage = true;
                GetNode<Timer>("ChangeWeaponTimer").Start();
            }
            else if (Input.IsActionJustPressed("SelectRifle"))
            {
                _CurrentWeaponName = "Rifle";
                Node3D weapon = WeaponScenes[WeaponsIndexes[_CurrentWeaponName]].Instantiate<Node3D>();
                Node3D weaponContainer = GetNode<Node3D>("RotationHelper/Weapon");
                weaponContainer.GetChild(0).QueueFree();
                weaponContainer.AddChild(weapon);

                ChangeWeaponStage = true;
                GetNode<Timer>("ChangeWeaponTimer").Start();
            }
        }
    }
    private void Movement(double delta)
    {
        Vector3 velocity = Velocity;
        if (!IsOnFloor()) velocity.Y -= gravity * (float)delta;

        if (Input.IsActionJustPressed("jump") && IsOnFloor()) velocity.Y = JumpVelocity;

        if (Input.IsActionPressed("sprint") && IsOnFloor()) Speed = _sprintSpeed;
        else Speed = 5f;

        if (Input.IsActionPressed("sit_down") && IsOnFloor())
        {
            Scale = new Vector3(1, 0.6f, 1);
            Speed = _sitDownSpeed;
        }
        else if (!Input.IsActionPressed("sprint"))
        {
            Speed = 5f;
            Scale = new Vector3(1, 1, 1);
        }

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
    private void Fire()
    {
        if (Input.IsActionJustPressed("fire"))
        {
            IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            currentWeapon.Shoot();
            if (currentWeapon.IsSoundLoopable)
            {
                Timer shootTimer = GetNode<Timer>("ShootTimer");
                shootTimer.WaitTime = currentWeapon.IntervalBetweenShots;
                shootTimer.Start();
            }
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
            GetNode<AnimationPlayer>("AnimationPlayer").Play(currentWeapon.AnimationName);
            TakeShot();
        }

        bool isWeaponExistInScene = GetNode<Node3D>("RotationHelper/Weapon").GetChild(0) != null;
        if (!Input.IsActionPressed("fire") && isWeaponExistInScene)
        {
            IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
            if (currentWeapon.IsSoundLoopable)
            {
                GetNode<Timer>("ShootTimer").Stop();
                GD.Print("currentWeapon");
            }
        }
    }
    private void TakeShot() // for bullets
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
        IWeapon currentWeapon = GetNode<IWeapon>($"RotationHelper/Weapon/{WeaponNames[_CurrentWeaponName]}");
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetNode<AnimationPlayer>("AnimationPlayer").Play(currentWeapon.AnimationName);
        GetNode<IWeapon>($"RotationHelper/Weapon/{currentWeapon.WeaponName}").Shoot();
        GD.Print("OnShootTimerTimeout");
        TakeShot();
    }
    public override void _Input(InputEvent @event)
    {
        Node3D RotationHelper = GetNode<Node3D>("RotationHelper");
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            RotateY(-mouseEvent.Relative.X * MouseSensitivity);
            RotationHelper.RotateX(-mouseEvent.Relative.Y * MouseSensitivity);
            Vector3 cameraRot = RotationHelper.RotationDegrees;
            cameraRot.X = Math.Clamp(cameraRot.X, -70, 70);
            RotationHelper.RotationDegrees = cameraRot;
        }
        if (@event is InputEventKey)
        {
            InputEventKey keyEvent = @event as InputEventKey;
            ChangeWeapon(keyEvent.Keycode.ToString());
        }
    }
}