namespace TopDownArenaShooter.MainMenu;

using System;
using Godot;
using TopDownArenaShooter.Shared.ScenePaths;

public partial class MainMenu : Control
{
    private Button _startButton;
    private Button _quitButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (GetNode<VBoxContainer>("VBoxContainer") is VBoxContainer container)
        {
            _startButton = container.GetNode<Button>("StartButton");
            _startButton.Pressed += OnStartButtonPressed;
            _startButton.CallDeferred(MethodName.GrabFocus);
            _startButton.MouseEntered += () => _startButton.GrabFocus();

            _quitButton = container.GetNode<Button>("QuitButton");
            _quitButton.Pressed += OnQuitButtonPressed;
            _quitButton.MouseEntered += () => _quitButton.GrabFocus();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile(ScenePaths.MainPath());
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
