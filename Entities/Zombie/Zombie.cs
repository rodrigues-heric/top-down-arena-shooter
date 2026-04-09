namespace TopDownArenaShooter.Entities.Zombie;

using System;
using Godot;
using TopDownArenaShooter.Shared.Scripts.Interfaces;

public partial class Zombie : CharacterBody2D, IDamageable
{
    private const float MaxHealth = 100.0f;
    private const float AttackPower = 20.0f;
    private const float RotationSpeed = 1.0f;
    private const float MoveSpeed = 50.0f;

    private Area2D _hitbox;
    private Timer _attackTimer;

    private float _currentHealth;
    private ProgressBar _healthBar;
    private Node2D _player;
    private CollisionShape2D _body;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
        _body = GetNode<CollisionShape2D>("CollisionShape2D");
        _hitbox = GetNode<Area2D>("Hitbox");
        _attackTimer = GetNode<Timer>("AttackTimer");

        _currentHealth = MaxHealth;
        _healthBar = GetNode<ProgressBar>("HealthBar");
        _healthBar.MaxValue = MaxHealth;
        _healthBar.Value = _currentHealth;

        _hitbox.AreaEntered += OnBodyEnteredHitBox;
        AddToGroup("Enemies");
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleLookAtPlayer((float)delta);
        HandleMovement();
        CheckForAttack();
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

    private void CheckForAttack()
    {
        if (_attackTimer.IsStopped())
        {
            Godot.Collections.Array<Node2D> bodies = _hitbox.GetOverlappingBodies();

            foreach (Node2D body in bodies)
            {
                if (body.IsInGroup("Player") && body is IDamageable player)
                {
                    Attack(player);
                    break;
                }
            }
        }
    }

    private void Attack(IDamageable target)
    {
        target.TakeDamage(AttackPower);
        _attackTimer.Start();
    }

    private void OnBodyEnteredHitBox(Node2D body)
    {
        if (body.IsInGroup("Player") && _attackTimer.IsStopped() && body is IDamageable player)
        {
            Attack(player);
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
