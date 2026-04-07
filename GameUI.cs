namespace TopDownArenaShooter.GameUI;

using System;
using Godot;
using TopDownArenaShooter.Prefabs.FVX.Flashlight;

public partial class GameUI : CanvasLayer
{
    private ProgressBar _batteryBar;
    private Flashlight _playerFlashlight;

    public override void _Ready()
    {
        _batteryBar = GetNode<ProgressBar>("BatteryBar");

        if (GetTree().GetFirstNodeInGroup("Player") is Node2D player)
        {
            _playerFlashlight = player.GetNode<Flashlight>("Flashlight");
        }
    }

    public override void _Process(double delta)
    {
        if (_playerFlashlight != null)
        {
            _batteryBar.Value = _playerFlashlight.GetBatteryPercent();
            UpdateBarColor();
        }
    }

    private void UpdateBarColor()
    {
        if (_batteryBar.Value < 20)
        {
            _batteryBar.Modulate = Colors.Red;
        }
        else
        {
            _batteryBar.Modulate = Colors.Orange;
        }
    }
}
