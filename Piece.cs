using Godot;
using System;
using System.Collections.Generic;
using CustomConstants;

public partial class Piece : Node2D
{
	private int _tileID;

	// | Type | Value       |
	// | ---- | ----------- |
	// |  -4  | BLACK_100PX |
	// |  -3  | BLACK_75PX  |
	// |  -2  | BLACK_50PX  |
	// |  -1  | BLACK_25PX  |
	// |  +4  | WHITE_100PX |
	// |  +3  | WHITE_75PX  |
	// |  +2  | WHITE_50PX  |
	// |  +1  | WHITE_25PX  |
	private int _type;
	private TileTypes _tileType;

	public Piece()
	{
		_tileID = -1;
		_type = 0;
		_tileType = TileTypes.NONE;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public int TileID
	{
		get
		{
			return _tileID;
		}
		set
		{
			_tileID = value;
		}
	}

	public int Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public TileTypes TileType
	{
		get
		{
			return _tileType;
		}
		set
		{
			_tileType = value;
		}
	}

	public void SetTexture(Texture2D texture)
	{
		GetNode<TextureRect>("Icon").Texture = texture;
	}

	[Signal]
	public delegate void PieceSelectedEventHandler(Piece piece);

	private void OnGUIInput(InputEvent @event)
	{
		if (@event.IsActionPressed("mouse_left"))
		{
			EmitSignal(SignalName.PieceSelected, this);
		}
	}
}
