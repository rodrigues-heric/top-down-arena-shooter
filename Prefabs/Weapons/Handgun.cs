namespace TopDownArenaShooter.Prefabs.Weapons.Handgun;

using System;
using Godot;

public partial class Handgun : Node2D
{
    private const float FireRate = 0.25f;
    private const float ReloadTime = 1.5f;
    private const float MuzzleDuration = 0.15f;
    private const int MaxAmmo = 7;

    private int _currentAmmo;
    private bool _isReloading;

    private Timer _fireRateTimer;
    private Timer _reloadTimer;
    private Marker2D _muzzle;
    private Timer _muzzleTimer;

    public int GetCurrentAmmo() => _currentAmmo;

    public bool IsReloading() => _isReloading;

    public int GetMaxAmmo() => MaxAmmo;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _currentAmmo = MaxAmmo;
        _isReloading = false;

        _fireRateTimer = GetNode<Timer>("FireRateTimer");
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        _muzzle = GetNode<Marker2D>("Muzzle");
        _muzzleTimer = GetNode<Timer>("MuzzleTimer");

        _muzzle.Visible = false;
        _reloadTimer.Timeout += OnReloadFinished;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        HandleMuzzleVisibility();
    }

    public void Shoot()
    {
        if (IsAbleToShoot())
        {
            _currentAmmo -= 1;
            _fireRateTimer.Start(FireRate);
            _muzzleTimer.Start(MuzzleDuration);
        }
    }

    public void StartReload()
    {
        if (IsAbleToReload())
        {
            _isReloading = true;
            _reloadTimer.Start(ReloadTime);
        }
    }

    private void OnReloadFinished()
    {
        _currentAmmo = MaxAmmo;
        _isReloading = false;
    }

    private void HandleMuzzleVisibility()
    {
        _muzzle.Visible = !_muzzleTimer.IsStopped();
    }

    private bool IsAbleToShoot()
    {
        return !(_isReloading || !_fireRateTimer.IsStopped() || _currentAmmo <= 0);
    }

    private bool IsAbleToReload()
    {
        return !(_isReloading || _currentAmmo == MaxAmmo);
    }
}
