namespace TopDownArenaShooter.Entities.Player;

using System;
using Godot;

public partial class Player : CharacterBody2D
{
    private const String MoveLeft = "move_left";
    private const String MoveRight = "move_right";
    private const String MoveUp = "move_up";
    private const String MoveDown = "move_down";

    public float Speed { get; set; } = 200.0f;

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

    // DEBUG: aim visual debug, remove later
    public override void _Draw()
    {
        Vector2 mousePosition = GetLocalMousePosition();
        Vector2 direction = mousePosition - Vector2.Zero;

        DrawLine(Vector2.Zero, direction, Colors.Yellow, 2.0f);
        DrawCircle(direction, 3.0f, Colors.Red);
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
