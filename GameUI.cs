namespace TopDownArenaShooter.GameUI;

using System;
using Godot;
using TopDownArenaShooter.Entities.Player;
using TopDownArenaShooter.Prefabs.FVX.Flashlight;
using TopDownArenaShooter.Prefabs.Weapons.Handgun;

public partial class GameUI : CanvasLayer
{
    private ProgressBar _batteryBar;
    private Label _ammoLabel;
    private Flashlight _playerFlashlight;
    private Handgun _playerHandgun;
    private Player _player;
    private ProgressBar _playerHealthBar;

    public override void _Ready()
    {
        _batteryBar = GetNode<ProgressBar>("BatteryBar");
        _ammoLabel = GetNode<Label>("AmmoLabel");

        _player = GetNode<Player>("../Player");
        _playerFlashlight = _player.GetNode<Flashlight>("Flashlight");
        _playerHandgun = _player.GetNode<Handgun>("Handgun");

        _playerHealthBar = GetNode<ProgressBar>("PlayerHealth");
        _playerHealthBar.Modulate = Colors.Green;
    }

    public override void _Process(double delta)
    {
        UpdateBatteryBar();
        UpdateAmmoLabel();
        UpdatePlayerHealth();
    }

    private void UpdatePlayerHealth()
    {
        if (_playerHealthBar != null && _player != null)
        {
            _playerHealthBar.Value = _player.GetCurrentHealth();

            if (_playerHealthBar.Value < 100)
                _playerHealthBar.Modulate = Colors.Green;
            if (_playerHealthBar.Value < 50)
                _playerHealthBar.Modulate = Colors.Yellow;
            if (_playerHealthBar.Value < 20)
                _playerHealthBar.Modulate = Colors.Red;
        }
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
