using Godot;
using System;
using CustomConstants;
using System.Collections.Generic;

public partial class Tile : Control
{
	private int _tileID;
	private TileTypes _type;
	private ColorRect _filter;
	private TileStates _state;

	private Stack<Piece> _pieces;

	public Tile()
	{
		_tileID = -1;
		_type = TileTypes.NONE;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_filter = GetNode<ColorRect>("Filter");
		_state = TileStates.NONE;
		_pieces = new Stack<Piece>();
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

	public TileTypes Type
	{
		get
		{
			return _type;
		}
		set
		{
			if (value != TileTypes.PLAYGROUND)
			{
				GetNode<TextureRect>("Icon").Texture = null;
			}
			_type = value;
		}
	}

	public TileStates State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}

	public Stack<Piece> Pieces
	{
		get
		{
			return _pieces;
		}
		set
		{
			_pieces = value;
		}
	}

	public void SetFilter(TileStates state = TileStates.NONE)
	{
		_state = state;
		switch (state)
		{
			case TileStates.NONE:
				_filter.Color = new Color(0, 0, 0, 0);
				break;
			case TileStates.LEGITIMATE:
				_filter.Color = new Color(0, 1, 0, (float)0.4);
				break;
			case TileStates.ILLEGITIMATE:
				_filter.Color = new Color(1, 0, 0, (float)0.4);
				break;
			default:
				break;
		}
	}

	[Signal]
	public delegate void TileClickedEventHandler(Tile tile);

	private void OnGUIInput(InputEvent @event)
	{
		if (@event.IsActionPressed("mouse_left"))
		{
			EmitSignal(SignalName.TileClicked, this);
		}
	}
}
