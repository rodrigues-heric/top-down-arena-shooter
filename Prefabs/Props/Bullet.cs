namespace TopDownArenaShooter.Prefabs.Props.Bullet;

using System;
using Godot;

public partial class Bullet : Area2D
{
    private const float Speed = 800.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").ScreenExited +=
            OnScreenExited;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Transform.X.Normalized() * Speed;
        GlobalPosition += velocity * (float)delta;
    }

    private void OnScreenExited()
    {
        DestroySelf();
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Enemies"))
        {
            DestroySelf();
        }

        if (body is StaticBody2D)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        SetDeferred(PropertyName.Monitoring, false);
        QueueFree();
    }
}
