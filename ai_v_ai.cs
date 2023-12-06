using Godot;
using System;

public partial class ai_v_ai : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnButtonPressed()
	{
		GD.Print("aivai");
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}
}
