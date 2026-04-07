namespace TopDownArenaShooter.Entities.Player;

using System;
using Godot;
using TopDownArenaShooter.Prefabs.FVX.Flashlight;
using TopDownArenaShooter.Prefabs.Weapons.Handgun;
using TopDownArenaShooter.Shared.Scripts.Interfaces;

public partial class Player : CharacterBody2D, IDamageable
{
    private const float MaxHealth = 100.0f;
    private const float HealRate = 0.01f;
    private const float HealCooldown = 5.0f;
    private float _currentHealth;

    private const String MoveLeft = "move_left";
    private const String MoveRight = "move_right";
    private const String MoveUp = "move_up";
    private const String MoveDown = "move_down";

    private const String LookLeft = "look_left";
    private const String LookRight = "look_right";
    private const String LookUp = "look_up";
    private const String LookDown = "look_down";

    private bool _isUsingGamepad = false;

    private const float Speed = 200.0f;

    private Flashlight _flashLight;
    private Handgun _handgun;
    private Timer _healCooldown;

    public float GetCurrentHealth() => _currentHealth;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _flashLight = GetNode<Flashlight>("Flashlight");
        _handgun = GetNode<Handgun>("Handgun");
        _healCooldown = GetNode<Timer>("HealCooldown");
        _currentHealth = MaxHealth;
        AddToGroup("Player");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        HandleShoot();
        HandleReload();
        HandleHeal();
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleCharacterMove();
        HandleCharacterLookAt();
    }

    public override void _Input(InputEvent @event)
    {
        HandleInputType(@event);
        HandleToggleFlashlight(@event);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, MaxHealth);
        _healCooldown.Stop();
        _healCooldown.Start(HealCooldown);

        if (_currentHealth <= 0)
            Die();
    }

    private void HandleHeal()
    {
        if (_healCooldown.IsStopped())
        {
            _currentHealth = Mathf.Clamp(_currentHealth + HealRate, 0, MaxHealth);
        }
    }

    private void Die()
    {
        GD.Print("Im dead");
    }

    private void HandleShoot()
    {
        if (Input.IsActionPressed("shoot"))
        {
            _handgun.Shoot();
        }
    }

    private void HandleReload()
    {
        if (Input.IsActionPressed("reload"))
        {
            _handgun.StartReload();
        }
    }

    private void HandleInputType(InputEvent @event)
    {
        if (IsUsingJoypad(@event))
            _isUsingGamepad = true;
        else if (IsUsingKeyboard(@event))
            if (IsMouseMoving(@event))
                _isUsingGamepad = false;
    }

    private void HandleToggleFlashlight(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_flashlight"))
        {
            _flashLight?.ToggleLight();
        }
    }

    private static bool IsUsingKeyboard(InputEvent @event)
    {
        return @event is InputEventMouseMotion
            || @event is InputEventMouseButton
            || @event is InputEventKey;
    }

    private static bool IsMouseMoving(InputEvent @event)
    {
        return @event is InputEventMouseMotion mouseMotion && mouseMotion.Relative.Length() > 0.1f;
    }

    private static bool IsUsingJoypad(InputEvent @event)
    {
        return @event is InputEventJoypadMotion || @event is InputEventJoypadButton;
    }

    private void HandleCharacterMove()
    {
        Vector2 inputDirection = Input.GetVector(MoveLeft, MoveRight, MoveUp, MoveDown);
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }

    private void HandleCharacterLookAt()
    {
        Vector2 joyDirection = Input.GetVector(LookLeft, LookRight, LookUp, LookDown);

        if (_isUsingGamepad)
        {
            if (joyDirection.Length() > 0.1)
                Rotation = joyDirection.Angle();
        }
        else
            LookAt(GetGlobalMousePosition());
    }
}
