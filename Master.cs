using Godot;
using System;

public partial class Master : VBoxContainer
{
	private Game _gameNode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameNode = (Game)GetNode<Control>("Game");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnStartGamePressed()
	{
		_gameNode.InitGame();
	}

	// False = Black, True = White
	private void OnFirstMoveToggled(bool toggled_on)
	{
		if (_gameNode.GameStarted)
		{
			return;
		}

		if (toggled_on)
		{
			_gameNode.Turn = 1;
		}
		else
		{
			_gameNode.Turn = -1;
		}
	}

	// False = Human, True = AI
	private void OnBlackPlayerToggled(bool toggled_on)
	{
		_gameNode.BlackPlayer = toggled_on;
	}

	// False = Human, True = AI
	private void OnWhitePlayerToggled(bool toggled_on)
	{
		_gameNode.WhitePlayer = toggled_on;
	}
}
