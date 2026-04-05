namespace TopDownArenaShooter.Entities.Player;

using System;
using Godot;

public partial class Player : CharacterBody2D
{
    private const String MoveLeft = "move_left";
    private const String MoveRight = "move_right";
    private const String MoveUp = "move_up";
    private const String MoveDown = "move_down";

    private const String LookLeft = "look_left";
    private const String LookRight = "look_right";
    private const String LookUp = "look_up";
    private const String LookDown = "look_down";

    private bool _isUsingGamepad = false;

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

    public override void _Input(InputEvent @event)
    {
        if (IsUsingJoypad(@event))
            _isUsingGamepad = true;
        else if (IsUsingKeyboard(@event))
            if (IsMouseMoving(@event))
                _isUsingGamepad = false;
    }

    private static bool IsUsingKeyboard(InputEvent @event)
    {
        return @event is InputEventMouseMotion
            || @event is InputEventMouseButton
            || @event is InputEventKey;
    }

    private static bool IsMouseMoving(InputEvent @event)
    {
        return @event is InputEventMouseMotion mouseMotion && mouseMotion.Relative.Length() > 0.1f;
    }

    private static bool IsUsingJoypad(InputEvent @event)
    {
        return @event is InputEventJoypadMotion || @event is InputEventJoypadButton;
    }

    private void HandleCharacterMove()
    {
        Vector2 inputDirection = Input.GetVector(MoveLeft, MoveRight, MoveUp, MoveDown);
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }

    private void HandleCharacterLookAt()
    {
        Vector2 joyDirection = Input.GetVector(LookLeft, LookRight, LookUp, LookDown);

        if (_isUsingGamepad)
        {
            if (joyDirection.Length() > 0.1)
                Rotation = joyDirection.Angle();
        }
        else
            LookAt(GetGlobalMousePosition());
    }
}
