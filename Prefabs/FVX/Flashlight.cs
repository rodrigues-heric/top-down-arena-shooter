namespace TopDownArenaShooter.Prefabs.FVX.Flashlight;

using System;
using Godot;

public partial class Flashlight : Node2D
{
    private const float MaxBattery = 100.0f;
    private const float ConsumptionRate = 5.0f;
    private const float RechargeRate = 2.0f;
    private const float LowBatteryLimit = 20.0f;

    private PointLight2D _light;
    private Timer _flickerTimer;
    private float _currentBattery;
    private bool _isOn = false;

    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("PointLight2D");
        _light.Enabled = false;

        _flickerTimer = GetNode<Timer>("FlickerTimer");
        _flickerTimer.Timeout += OnFlickerTimeout;

        _currentBattery = MaxBattery;
    }

    public override void _Process(double delta)
    {
        HandleBattery((float)delta);
        UpdateVisuals();
    }

    public void ToggleLight()
    {
        if (!_isOn && _currentBattery <= 0)
            return;

        _isOn = !_isOn;

        if (!_isOn)
        {
            _flickerTimer.Stop();
            _light.Enabled = false;
        }
    }

    private void HandleBattery(float delta)
    {
        if (_isOn && _currentBattery > 0)
        {
            _currentBattery -= ConsumptionRate * delta;

            if (_currentBattery <= 0)
            {
                _isOn = false;
                _flickerTimer.Stop();
            }
        }
        else
        {
            _currentBattery = Mathf.MoveToward(_currentBattery, MaxBattery, RechargeRate * delta);
        }
        _currentBattery = Mathf.Clamp(_currentBattery, 0, MaxBattery);
    }

    private void UpdateVisuals()
    {
        if (!_isOn)
        {
            _flickerTimer.Stop();
            _light.Enabled = false;
            return;
        }

        if (_currentBattery < LowBatteryLimit)
        {
            HandleLowBattery();
        }
        else
        {
            HandleHighBattery();
        }
    }

    private void HandleLowBattery()
    {
        if (_flickerTimer.IsStopped())
        {
            _flickerTimer.Start(0.15f);
        }
    }

    private void HandleHighBattery()
    {
        _flickerTimer.Stop();
        _light.Enabled = true;
    }

    private void OnFlickerTimeout()
    {
        if (_isOn && _currentBattery < LowBatteryLimit)
        {
            _light.Enabled = !_light.Enabled;
        }
    }
}
