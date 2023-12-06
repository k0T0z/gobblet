using Godot;
using System;
using CustomConstants;

public partial class Tile : Control
{
	private int _tileID;
	private TileTypes _type;
	private ColorRect _filter;
	private TileStates _state;
	
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
			if (value != TileTypes.PLAYGROUND) {
				GetNode<TextureRect>("Icon").Texture = null;
			}
			_type = value;
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
			case TileStates.FREE:
				_filter.Color = new Color(0, 1, 0, (float)0.4);
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
