namespace TopDownArenaShooter.Prefabs.FVX.Flashlight;

using System;
using Godot;

public partial class Flashlight : Node2D
{
    private PointLight2D _light;

    private bool _isOn = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("PointLight2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        UpdateVisuals();
    }

    public void ToggleLight() => _isOn = !_isOn;

    private void UpdateVisuals()
    {
        _light.Visible = _isOn;
    }
}
