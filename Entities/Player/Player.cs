namespace TopDownArenaShooter.Entities.Player;

using System;
using Godot;

public partial class Player : CharacterBody2D
{
    private const String MoveLeft = "move_left";
    private const String MoveRight = "move_right";
    private const String MoveUp = "move_up";
    private const String MoveDown = "move_down";

    private const float Speed = 200.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleCharacterMove();
        HandleCharacterLookAt();
    }

    private void HandleCharacterMove()
    {
        Vector2 inputDirection = Input.GetVector(MoveLeft, MoveRight, MoveUp, MoveDown);
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }

    private void HandleCharacterLookAt()
    {
        LookAt(GetGlobalMousePosition());
    }
}
