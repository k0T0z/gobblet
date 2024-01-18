using Godot;
using System;
using System.Collections.Generic;
using CustomConstants;

public partial class Game : Control
{
	private PackedScene _tileScene;
	private GridContainer _playgroundBoardTilesNode;
	private PackedScene _pieceScene;
	private Control _playgroundBoardNode;
	private List<Tile> _playgroundBoardTiles;

	private List<Tile> _whiteBoardTiles;
	private List<Tile> _blackBoardTiles;

	private PackedScene _whiteBoardScene;
	private PackedScene _blackBoardScene;

	private VBoxContainer _whiteBoardTilesNode;
	private VBoxContainer _blackBoardTilesNode;
	private Control _whiteBoardNode;
	private Control _blackBoardNode;

	private readonly Vector2 _iconOffset = new(80, 80);

	private Piece _pieceSelected;
	private int _turn;

	private List<int> _extraLegitimateTiles;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileScene = GD.Load<PackedScene>("res://scenes/tile.tscn");
		_playgroundBoardTilesNode = GetNode<GridContainer>("Board/BoardGrid");
		_pieceScene = GD.Load<PackedScene>("res://scenes/piece.tscn");
		_playgroundBoardNode = GetNode<Control>("Board");

		_playgroundBoardTiles = new List<Tile>();

		_pieceSelected = null;
		_turn = 0;
		_extraLegitimateTiles = new List<int>();
		HidePossibleMoves();

		for (int i = 0; i < 16; i++)
		{
			CreateTile(i);
		}

		_whiteBoardTilesNode = GetNode<VBoxContainer>("WhiteBoard/Board/BoardVBox");
		_blackBoardTilesNode = GetNode<VBoxContainer>("BlackBoard/Board/BoardVBox");
		_whiteBoardNode = GetNode<Control>("WhiteBoard/Board");
		_blackBoardNode = GetNode<Control>("BlackBoard/Board");

		_whiteBoardTiles = new List<Tile>();
		_blackBoardTiles = new List<Tile>();

		for (int i = 0; i < 3; i++)
		{
			CreateWhiteTile(i);
			CreateBlackTile(i);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CreateTile(int id)
	{
		Tile tile = (Tile)_tileScene.Instantiate();
		_playgroundBoardTiles.Add(tile);
		tile.TileID = id;
		tile.Type = TileTypes.PLAYGROUND;
		_playgroundBoardTilesNode.AddChild(tile);
		tile.TileClicked += OnTileClicked;
	}

	private void CreateWhiteTile(int id)
	{
		Tile tile = (Tile)_tileScene.Instantiate();
		_whiteBoardTiles.Add(tile);
		tile.TileID = id;
		tile.Type = TileTypes.WHITE;
		_whiteBoardTilesNode.AddChild(tile);
		tile.TileClicked += OnTileClicked;
	}

	private void CreateBlackTile(int id)
	{
		Tile tile = (Tile)_tileScene.Instantiate();
		_blackBoardTiles.Add(tile);
		tile.TileID = id;
		tile.Type = TileTypes.BLACK;
		_blackBoardTilesNode.AddChild(tile);
		tile.TileClicked += OnTileClicked;
	}

	private void InitWhiteBoardPieces()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Piece piece = (Piece)_pieceScene.Instantiate();
				piece.TileType = TileTypes.WHITE;
				int type = 0;
				switch (i)
				{
					case 0: type = 1; piece.SetTexture(GD.Load<Texture2D>("res://assets/white_25px.svg")); break;
					case 1: type = 2; piece.SetTexture(GD.Load<Texture2D>("res://assets/white_50px.svg")); break;
					case 2: type = 3; piece.SetTexture(GD.Load<Texture2D>("res://assets/white_75px.svg")); break;
					case 3: type = 4; piece.SetTexture(GD.Load<Texture2D>("res://assets/white_100px.svg")); break;
					default: break;
				}
				piece.Type = type;
				_whiteBoardTiles[j].Pieces.Push(piece);
			}
		}
	}

	private void InitBlackBoardPieces()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Piece piece = (Piece)_pieceScene.Instantiate();
				piece.TileType = TileTypes.BLACK;
				int type = 0;
				switch (i)
				{
					case 0: type = -1; piece.SetTexture(GD.Load<Texture2D>("res://assets/black_25px.svg")); break;
					case 1: type = -2; piece.SetTexture(GD.Load<Texture2D>("res://assets/black_50px.svg")); break;
					case 2: type = -3; piece.SetTexture(GD.Load<Texture2D>("res://assets/black_75px.svg")); break;
					case 3: type = -4; piece.SetTexture(GD.Load<Texture2D>("res://assets/black_100px.svg")); break;
					default: break;
				}
				piece.Type = type;
				_blackBoardTiles[j].Pieces.Push(piece);
			}
		}
	}

	public void InitGame()
	{
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < _playgroundBoardTiles[i].Pieces.Count; j++)
			{
				_playgroundBoardNode.RemoveChild(_playgroundBoardTiles[i].Pieces.Pop()); // .QueueFree()
			}
		}

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < _whiteBoardTiles[i].Pieces.Count; j++)
			{
				_whiteBoardNode.RemoveChild(_whiteBoardTiles[i].Pieces.Pop()); // .QueueFree()
			}
			for (int j = 0; j < _blackBoardTiles[i].Pieces.Count; j++)
			{
				_blackBoardNode.RemoveChild(_blackBoardTiles[i].Pieces.Pop()); // .QueueFree()
			}
		}

		InitWhiteBoardPieces();
		InitBlackBoardPieces();

		for (int i = 0; i < 3; i++)
		{
			Piece whitePiece = _whiteBoardTiles[i].Pieces.Peek();
			Piece blackPiece = _blackBoardTiles[i].Pieces.Peek();
			_whiteBoardNode.AddChild(whitePiece);
			_blackBoardNode.AddChild(blackPiece);
			whitePiece.GlobalPosition = _whiteBoardTiles[i].GlobalPosition + _iconOffset;
			blackPiece.GlobalPosition = _blackBoardTiles[i].GlobalPosition + _iconOffset;
			whitePiece.TileID = i;
			blackPiece.TileID = i;
			whitePiece.PieceSelected += OnPieceSelected;
			blackPiece.PieceSelected += OnPieceSelected;
		}

		_turn = -1; // Black starts the game.
	}

	private void OnTileClicked(Tile tile)
	{
		// If I pressed an empty tile.
		if (_pieceSelected == null)
		{
			return;
		}

		// If the destination tile is not playground.
		if (tile.Type != TileTypes.PLAYGROUND)
		{
			_pieceSelected = null;
			HidePossibleMoves();
			return;
		}

		MovePiece(_pieceSelected, tile.TileID);

		_pieceSelected = null;
		_turn *= -1; // Change the turn.
		HidePossibleMoves();
	}

	public void MovePiece(Piece piece, int location)
	{
		// piece is the source piece that we need to move.
		// If we will add a new piece to the ground.
		if (piece.TileType != TileTypes.PLAYGROUND)
		{
			switch (piece.TileType)
			{
				case TileTypes.WHITE:
					{
						_whiteBoardNode.RemoveChild(_whiteBoardTiles[piece.TileID].Pieces.Pop());
						if (_whiteBoardTiles[piece.TileID].Pieces.Count != 0)
						{
							Piece whitePiece = _whiteBoardTiles[piece.TileID].Pieces.Peek();
							_whiteBoardNode.AddChild(whitePiece);
							whitePiece.GlobalPosition = _whiteBoardTiles[piece.TileID].GlobalPosition + _iconOffset;
							whitePiece.TileID = piece.TileID;
							whitePiece.PieceSelected += OnPieceSelected;
						}
						break;
					}
				case TileTypes.BLACK:
					{
						_blackBoardNode.RemoveChild(_blackBoardTiles[piece.TileID].Pieces.Pop());
						if (_blackBoardTiles[piece.TileID].Pieces.Count != 0)
						{
							Piece blackPiece = _blackBoardTiles[piece.TileID].Pieces.Peek();
							_blackBoardNode.AddChild(blackPiece);
							blackPiece.GlobalPosition = _blackBoardTiles[piece.TileID].GlobalPosition + _iconOffset;
							blackPiece.TileID = piece.TileID;
							blackPiece.PieceSelected += OnPieceSelected;
						}
						break;
					}
				default: break;
			}
			_playgroundBoardNode.AddChild(piece);
			piece.GlobalPosition = _playgroundBoardTiles[location].GlobalPosition + _iconOffset;
			_playgroundBoardTiles[location].Pieces.Push(piece);
			piece.TileType = TileTypes.PLAYGROUND;
		}
		// If we will move a piece that is on the ground.
		else
		{
			_playgroundBoardNode.MoveChild(piece, -1); // Move the child to the top on the tree.
			_playgroundBoardTiles[piece.TileID].Pieces.Pop();
			_playgroundBoardTiles[location].Pieces.Push(piece);
			var tween = GetTree().CreateTween();
			tween.TweenProperty(piece, "global_position", _playgroundBoardTiles[location].GlobalPosition + _iconOffset, 0.5);
		}

		piece.TileID = location;
	}

	private void OnPieceSelected(Piece piece)
	{
		bool extraLegitimateTiles = false;

		// If I am not selecting any piece.
		if (_pieceSelected == null)
		{
			HideIllegitimateMoves();

			// If it is not my turn.
			if (_turn < 0 && piece.Type > 0 || _turn > 0 && piece.Type < 0)
			{
				ShowIllegitimateMoves(piece, piece);
				return;
			}

			_pieceSelected = piece;

			// TODO: Check if there are any 3 lined pieces to give the player the option to stack on them before showing the possible moves.
			extraLegitimateTiles = CheckExtraLegitimateTiles();

			ShowPossibleMoves(extraLegitimateTiles);
			return;
		}

		// If the source and destination pieces are the same.
		if (_pieceSelected.TileID == piece.TileID && _pieceSelected.TileType == piece.TileType)
		{
			ShowIllegitimateMoves(_pieceSelected, piece);
			_pieceSelected = null;
			HidePossibleMoves();
			return;
		}

		// If the destination tile is not in the playground.
		if (piece.TileType != TileTypes.PLAYGROUND)
		{
			ShowIllegitimateMoves(_pieceSelected, piece);
			_pieceSelected = null;
			HidePossibleMoves();
			return;
		}

		// If the place is not empty and has bigger piece.
		if (_playgroundBoardTiles[piece.TileID].Pieces.Count != 0 &&
				Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[piece.TileID].Pieces.Peek().Type))
		{
			ShowIllegitimateMoves(_pieceSelected, piece);
			_pieceSelected = null;
			HidePossibleMoves();
			return;
		}

		// If the place is not empty and has smaller piece and there are no extra legitimate tiles.
		if (_playgroundBoardTiles[piece.TileID].Pieces.Count != 0 &&
				Math.Abs(_pieceSelected.Type) > Math.Abs(_playgroundBoardTiles[piece.TileID].Pieces.Peek().Type) &&
				!extraLegitimateTiles)
		{
			ShowIllegitimateMoves(_pieceSelected, piece);
			_pieceSelected = null;
			HidePossibleMoves();
			return;
		}

		OnTileClicked(_playgroundBoardTiles[piece.TileID]);
	}

	private void ShowPossibleMoves(bool extraLegitimateTiles = false)
	{
		for (int i = 0; i < _playgroundBoardTiles.Count; i++)
		{
			// If I am in the same location in the playground board.
			if (_pieceSelected.TileType == TileTypes.PLAYGROUND && _pieceSelected.TileID == i) continue;

			// If the place is not empty.
			if (_playgroundBoardTiles[i].Pieces.Count != 0) continue;

			_playgroundBoardTiles[i].SetFilter(TileStates.LEGITIMATE);
		}

		// If there are no extra legitimate tiles.
		if (!extraLegitimateTiles) return;

		for (int i = 0; i < _extraLegitimateTiles.Count; i++)
		{
			_playgroundBoardTiles[_extraLegitimateTiles[i]].SetFilter(TileStates.LEGITIMATE);
		}
	}

	private void HidePossibleMoves()
	{
		for (int i = 0; i < _playgroundBoardTiles.Count; i++)
		{
			if (_playgroundBoardTiles[i].State == TileStates.ILLEGITIMATE) continue;

			_playgroundBoardTiles[i].SetFilter(TileStates.NONE);
		}

		_extraLegitimateTiles.Clear();
	}

	private void ShowIllegitimateMoves(Piece source, Piece destination)
	{
		switch (source.TileType)
		{
			case TileTypes.BLACK: _blackBoardTiles[source.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			case TileTypes.PLAYGROUND: _playgroundBoardTiles[source.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			case TileTypes.WHITE: _whiteBoardTiles[source.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			default: break;
		}

		switch (destination.TileType)
		{
			case TileTypes.BLACK: _blackBoardTiles[destination.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			case TileTypes.PLAYGROUND: _playgroundBoardTiles[destination.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			case TileTypes.WHITE: _whiteBoardTiles[destination.TileID].SetFilter(TileStates.ILLEGITIMATE); break;
			default: break;
		}
	}

	private void HideIllegitimateMoves()
	{
		for (int i = 0; i < _blackBoardTiles.Count; i++)
		{
			if (_blackBoardTiles[i].State != TileStates.ILLEGITIMATE) continue;
			_blackBoardTiles[i].SetFilter(TileStates.NONE);
		}

		for (int i = 0; i < _playgroundBoardTiles.Count; i++)
		{
			if (_playgroundBoardTiles[i].State != TileStates.ILLEGITIMATE) continue;
			_playgroundBoardTiles[i].SetFilter(TileStates.NONE);
		}

		for (int i = 0; i < _whiteBoardTiles.Count; i++)
		{
			if (_whiteBoardTiles[i].State != TileStates.ILLEGITIMATE) continue;
			_whiteBoardTiles[i].SetFilter(TileStates.NONE);
		}
	}

	// TODO: Fix me.
	private bool CheckExtraLegitimateTiles()
	{
		bool extraLegitimateTiles = false;
		for (int i = 0; i < 4; i++)
		{
			int counter = 0;

			// Check for rows.
			for (int j = 0; j < 4; j++)
			{
				// If the place is empty.
				if (_playgroundBoardTiles[i * 4 + j].Pieces.Count == 0) continue;

				// If the place has bigger piece.
				if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type)) continue;

				// If the piece is not mine.
				if (_turn < 0 && _playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type > 0 ||
						_turn > 0 && _playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type < 0)
				{
					counter++;
				}
			}

			// The counter must not be >= 4 beecause in this case the game will be over.
			if (counter >= 3)
			{
				extraLegitimateTiles = true;
				for (int j = 0; j < 4; j++)
				{
					// If the place is empty.
					if (_playgroundBoardTiles[i * 4 + j].Pieces.Count == 0) continue;

					// If the place has bigger piece.
					if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type)) continue;

					// If the piece is not mine.
					if (_turn < 0 && _playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type > 0 ||
							_turn > 0 && _playgroundBoardTiles[i * 4 + j].Pieces.Peek().Type < 0)
					{
						_extraLegitimateTiles.Add(i * 4 + j);
					}
				}
				counter = 0;
			}

			// Check for columns.
			for (int j = 0; j < 4; j++)
			{

				// If the place is empty for columns.
				if (_playgroundBoardTiles[j * 4 + i].Pieces.Count == 0) continue;

				// If the place has bigger piece for columns.
				if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type)) continue;

				// If the piece is not mine.
				if (_turn < 0 && _playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type > 0 ||
						_turn > 0 && _playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type < 0)
				{
					counter++;
				}
			}

			// The counter must not be >= 4 beecause in this case the game will be over.
			if (counter >= 3)
			{
				extraLegitimateTiles = true;
				for (int j = 0; j < 4; j++)
				{

					// If the place is empty for columns.
					if (_playgroundBoardTiles[j * 4 + i].Pieces.Count == 0) continue;

					// If the place has bigger piece for columns.
					if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type)) continue;

					// If the piece is not mine.
					if (_turn < 0 && _playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type > 0 ||
							_turn > 0 && _playgroundBoardTiles[j * 4 + i].Pieces.Peek().Type < 0)
					{
						_extraLegitimateTiles.Add(j * 4 + i);
					}
				}
			}
		}

		int temp_counter = 0;

		// Check -45 slope diagonal.
		for (int j = 0; j < 4; j++)
		{
			// If the place is empty for diagonals.
			if (_playgroundBoardTiles[j * 4 + j].Pieces.Count == 0) continue;

			// If the place has bigger piece for diagonals.
			if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type)) continue;

			// If the piece is not mine.
			if (_turn < 0 && _playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type > 0 ||
					_turn > 0 && _playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type < 0)
			{
				temp_counter++;
			}
		}

		// The counter must not be >= 4 beecause in this case the game will be over.
		if (temp_counter >= 3)
		{
			extraLegitimateTiles = true;
			for (int j = 0; j < 4; j++)
			{
				// If the place is empty for diagonals.
				if (_playgroundBoardTiles[j * 4 + j].Pieces.Count == 0) continue;

				// If the place has bigger piece for diagonals.
				if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type)) continue;

				// If the piece is not mine.
				if (_turn < 0 && _playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type > 0 ||
						_turn > 0 && _playgroundBoardTiles[j * 4 + j].Pieces.Peek().Type < 0)
				{
					_extraLegitimateTiles.Add(j * 4 + j);
				}
			}
			temp_counter = 0;
		}

		// Check +45 slope diagonal.
		for (int j = 0; j < 4; j++)
		{
			// If the place is empty for diagonals.
			if (_playgroundBoardTiles[j * 4 - j + 3].Pieces.Count == 0) continue;

			// If the place has bigger piece for diagonals.
			if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type)) continue;

			// If the piece is not mine.
			if (_turn < 0 && _playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type > 0 ||
					_turn > 0 && _playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type < 0)
			{
				temp_counter++;
			}
		}

		// The counter must not be >= 4 beecause in this case the game will be over.
		if (temp_counter >= 3)
		{
			extraLegitimateTiles = true;
			for (int j = 0; j < 4; j++)
			{
				// If the place is empty for diagonals.
				if (_playgroundBoardTiles[j * 4 - j + 3].Pieces.Count == 0) continue;

				// If the place has bigger piece for diagonals.
				if (Math.Abs(_pieceSelected.Type) <= Math.Abs(_playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type)) continue;

				// If the piece is not mine.
				if (_turn < 0 && _playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type > 0 ||
						_turn > 0 && _playgroundBoardTiles[j * 4 - j + 3].Pieces.Peek().Type < 0)
				{
					_extraLegitimateTiles.Add(j * 4 - j + 3);
				}
			}
		}

		// GD.Print("Extra Legitimate Tiles:");

		// for (int i = 0; i < _extraLegitimateTiles.Count; i++)
		// {
		// 	GD.Print(_extraLegitimateTiles[i]);
		// }

		// GD.Print("End of Extra Legitimate Tiles.");

		return extraLegitimateTiles;
	}
}
