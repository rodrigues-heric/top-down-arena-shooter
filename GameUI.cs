namespace TopDownArenaShooter.GameUI;

using System;
using Godot;
using TopDownArenaShooter.Prefabs.FVX.Flashlight;
using TopDownArenaShooter.Prefabs.Weapons.Handgun;

public partial class GameUI : CanvasLayer
{
    private ProgressBar _batteryBar;
    private Label _ammoLabel;
    private Flashlight _playerFlashlight;
    private Handgun _playerHandgun;

    public override void _Ready()
    {
        _batteryBar = GetNode<ProgressBar>("BatteryBar");
        _ammoLabel = GetNode<Label>("AmmoLabel");

        if (GetTree().GetFirstNodeInGroup("Player") is Node2D player)
        {
            _playerFlashlight = player.GetNode<Flashlight>("Flashlight");
            _playerHandgun = player.GetNode<Handgun>("Handgun");
        }
    }

    public override void _Process(double delta)
    {
        UpdateBatteryBar();
        UpdateAmmoLabel();
    }

    private void UpdateBatteryBar()
    {
        if (_playerFlashlight != null)
        {
            _batteryBar.Value = _playerFlashlight.GetBatteryPercent();
            UpdateBarColor();
        }
    }

    private void UpdateAmmoLabel()
    {
        if (_playerHandgun != null)
        {
            if (_playerHandgun.IsReloading())
            {
                _ammoLabel.Text = "Reloading...";
            }
            else
            {
                _ammoLabel.Text =
                    $"Ammo: {_playerHandgun.GetCurrentAmmo():D2}/{_playerHandgun.GetMaxAmmo():D2}";
            }
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
