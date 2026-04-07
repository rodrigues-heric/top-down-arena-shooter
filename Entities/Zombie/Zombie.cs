namespace TopDownArenaShooter.Entities.Zombie;

using System;
using Godot;
using TopDownArenaShooter.Shared.Scripts.Interfaces;

public partial class Zombie : CharacterBody2D, IDamageable
{
    private const float MaxHealth = 100.0f;
    private const float RotationSpeed = 1.0f;
    private const float MoveSpeed = 50.0f;

    private float _currentHealth;
    private ProgressBar _healthBar;
    private Node2D _player;
    private CollisionShape2D _body;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        _body = GetNode<CollisionShape2D>("CollisionShape2D");

        _currentHealth = MaxHealth;
        _healthBar = GetNode<ProgressBar>("HealthBar");
        _healthBar.MaxValue = MaxHealth;
        _healthBar.Value = _currentHealth;

        AddToGroup("Enemies");
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleLookAtPlayer((float)delta);
        HandleMovement();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _healthBar.Value = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        QueueFree();
    }

    private void HandleMovement()
    {
        if (_player != null)
        {
            Vector2 direction = new Vector2(1, 0).Rotated(_body.Rotation);
            Velocity = direction * MoveSpeed;
            MoveAndSlide();
        }
    }

    private void HandleLookAtPlayer(float delta)
    {
        if (_player != null)
        {
            Vector2 direction = _player.GlobalPosition - GlobalPosition;
            float targetAngle = direction.Angle();
            _body.Rotation = (float)
                Mathf.LerpAngle(_body.Rotation, targetAngle, RotationSpeed * delta);
        }
    }
}
