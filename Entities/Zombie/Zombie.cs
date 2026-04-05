namespace TopDownArenaShooter.Entities.Zombie;

using System;
using Godot;

public partial class Zombie : CharacterBody2D
{
    private const float RotationSpeed = 1.0f;

    private Node2D _player;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleLookAtPlayer((float)delta);
    }

    private void HandleLookAtPlayer(float delta)
    {
        if (_player != null)
        {
            Vector2 direction = _player.GlobalPosition - GlobalPosition;
            float targetAngle = direction.Angle();
            Rotation = (float)Mathf.LerpAngle(Rotation, targetAngle, RotationSpeed * delta);
        }
    }
}
