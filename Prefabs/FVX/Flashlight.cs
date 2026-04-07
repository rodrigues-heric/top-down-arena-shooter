namespace TopDownArenaShooter.Prefabs.FVX.Flashlight;

using System;
using Godot;

public partial class Flashlight : Node2D
{
    private float MaxBattery = 100.0f;
    private float ConsumptionRate = 5.0f;
    private float RechargeRate = 2.0f;

    private PointLight2D _light;
    private Timer _flickerTimer;
    private float _currentBattery;
    private bool _isOn = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("PointLight2D");
        _flickerTimer = GetNode<Timer>("FlickerTimer");
        _flickerTimer.Timeout += OnFlickerTimeout;
        _currentBattery = MaxBattery;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        HandleBattery((float)delta);
        UpdateVisuals();

        GD.Print("Battery: ", _currentBattery);
        GD.Print("Timer Stopped: ", _flickerTimer.IsStopped());
        GD.Print("Light On: ", _isOn);
    }

    public void ToggleLight()
    {
        _isOn = !_isOn;
    }

    private void HandleBattery(float delta)
    {
        if (_isOn && _currentBattery > 0)
        {
            _currentBattery -= ConsumptionRate * delta;
        }
        else
        {
            _currentBattery = Mathf.MoveToward(_currentBattery, MaxBattery, RechargeRate * delta);
        }
        _currentBattery = Mathf.Clamp(_currentBattery, 0, MaxBattery);
    }

    private void OnFlickerTimeout()
    {
        _light.Enabled = !_light.Enabled;
    }

    private void UpdateVisuals()
    {
        if (_currentBattery <= 0)
            HandleEmptyBattery();
        else if (_currentBattery < 15.0f)
            HandleLowBattery();
        else
            HandleHighBattery();
    }

    private void HandleEmptyBattery()
    {
        _flickerTimer.Stop();
        _isOn = false;
    }

    private void HandleLowBattery()
    {
        if (_flickerTimer.IsStopped())
        {
            _flickerTimer.Start(0.5f);
        }
    }

    private void HandleHighBattery()
    {
        _flickerTimer.Stop();
        _light.Visible = _isOn;
        _light.Enabled = _isOn;
    }
}
