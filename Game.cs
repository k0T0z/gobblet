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
	private List<Stack<Piece>> _playgroundBoardPieces;
	private List<string> _assetPaths;

	private List<Tile> _whiteBoardTiles;
	private List<Tile> _blackBoardTiles;
	private List<Stack<Piece>> _whiteBoardPieces;
	private List<Stack<Piece>> _blackBoardPieces;

	private PackedScene _whiteBoardScene;
	private PackedScene _blackBoardScene;

	private VBoxContainer _whiteBoardTilesNode;
	private VBoxContainer _blackBoardTilesNode;
	private Control _whiteBoardNode;
	private Control _blackBoardNode;

	private readonly Vector2 _iconOffset = new(80, 80);

	private Piece _pieceSelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileScene = GD.Load<PackedScene>("res://scenes/tile.tscn");
		_playgroundBoardTilesNode = GetNode<GridContainer>("Board/BoardGrid");
		_pieceScene = GD.Load<PackedScene>("res://scenes/piece.tscn");
		_playgroundBoardNode = GetNode<Control>("Board");

		_playgroundBoardTiles = new List<Tile>();
		_playgroundBoardPieces = new List<Stack<Piece>>();
		_assetPaths = new List<string>();

		/* 
			The rest of the code depends on this order in the `_assets` list.
			This is the same order as `CustomConstants.PieceTypes`.
		*/
		_assetPaths.Add("res://assets/white_25px.svg");
		_assetPaths.Add("res://assets/white_50px.svg");
		_assetPaths.Add("res://assets/white_75px.svg");
		_assetPaths.Add("res://assets/white_100px.svg");
		_assetPaths.Add("res://assets/black_25px.svg");
		_assetPaths.Add("res://assets/black_50px.svg");
		_assetPaths.Add("res://assets/black_75px.svg");
		_assetPaths.Add("res://assets/black_100px.svg");

		_pieceSelected = null;

		for (int i = 0; i < 16; i++)
		{
			CreateTile(i);
			_playgroundBoardPieces.Add(new Stack<Piece>()); // Initialize the `_pieceArray`.
		}

		_whiteBoardTilesNode = GetNode<VBoxContainer>("WhiteBoard/Board/BoardVBox");
		_blackBoardTilesNode = GetNode<VBoxContainer>("BlackBoard/Board/BoardVBox");
		_whiteBoardNode = GetNode<Control>("WhiteBoard/Board");
		_blackBoardNode = GetNode<Control>("BlackBoard/Board");

		_whiteBoardTiles = new List<Tile>();
		_blackBoardTiles = new List<Tile>();
		_whiteBoardPieces = new List<Stack<Piece>>();
		_blackBoardPieces = new List<Stack<Piece>>();

		for (int i = 0; i < 3; i++)
		{
			CreateWhiteTile(i);
			CreateBlackTile(i);
			_whiteBoardPieces.Add(new Stack<Piece>());
			_blackBoardPieces.Add(new Stack<Piece>());
		}

		InitWhiteBoardPieces();
		InitBlackBoardPieces();
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
				int type = -1;
				switch (i)
				{
					case 0: type = (int)PieceTypes.WHITE_25PX; break;
					case 1: type = (int)PieceTypes.WHITE_50PX; break;
					case 2: type = (int)PieceTypes.WHITE_75PX; break;
					case 3: type = (int)PieceTypes.WHITE_100PX; break;
					default: break;
				}
				piece.Type = type;
				piece.SetTexture(GD.Load<Texture2D>(_assetPaths[type]));
				_whiteBoardPieces[j].Push(piece);
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
				int type = -1;
				switch (i)
				{
					case 0: type = (int)PieceTypes.BLACK_25PX; break;
					case 1: type = (int)PieceTypes.BLACK_50PX; break;
					case 2: type = (int)PieceTypes.BLACK_75PX; break;
					case 3: type = (int)PieceTypes.BLACK_100PX; break;
					default: break;
				}
				piece.Type = type;
				piece.SetTexture(GD.Load<Texture2D>(_assetPaths[type]));
				_blackBoardPieces[j].Push(piece);
			}
		}
	}
	
	public void InitGame() {
		if (_whiteBoardPieces.Count == 0 || _blackBoardPieces.Count == 0) {
			return;
		}
		
		for (int i = 0; i < 3; i++)
		{
			Piece whitePiece = _whiteBoardPieces[i].Peek();
			Piece blackPiece = _blackBoardPieces[i].Peek();
			_whiteBoardNode.AddChild(whitePiece);
			_blackBoardNode.AddChild(blackPiece);
			whitePiece.GlobalPosition = _whiteBoardTiles[i].GlobalPosition + _iconOffset;
			blackPiece.GlobalPosition = _blackBoardTiles[i].GlobalPosition + _iconOffset;
			whitePiece.TileID = i;
			blackPiece.TileID = i;
			whitePiece.PieceSelected += OnPieceSelected;
			blackPiece.PieceSelected += OnPieceSelected;
		}
	}

	private void OnTileClicked(Tile tile)
	{
		if (_pieceSelected == null)
		{
			return;
		}
		
		if (tile.Type != TileTypes.PLAYGROUND) {
			_pieceSelected = null;
			return;
		}

		MovePiece(_pieceSelected, tile.TileID);

		_pieceSelected = null;
	}

	public void MovePiece(Piece piece, int location)
	{
		//if (_playgroundBoardPieces[location].Count != 0)
		//{
			//_playgroundBoardPieces[location].Peek().QueueFree();
			//_playgroundBoardPieces[location].Pop();
		//}
		
		// TODO: Solve problem of stacking in playground board.
		
		if (piece.TileType != TileTypes.PLAYGROUND) {
			switch (piece.TileType) {
				case TileTypes.WHITE: {
					_whiteBoardNode.RemoveChild(_whiteBoardPieces[piece.TileID].Pop());
					if (_whiteBoardPieces[piece.TileID].Count != 0) {
						Piece whitePiece = _whiteBoardPieces[piece.TileID].Peek();
						_whiteBoardNode.AddChild(whitePiece);
						whitePiece.GlobalPosition = _whiteBoardTiles[piece.TileID].GlobalPosition + _iconOffset;
						whitePiece.TileID = piece.TileID;
						whitePiece.PieceSelected += OnPieceSelected;
					}
					break;
				}
				case TileTypes.BLACK: {
					_blackBoardNode.RemoveChild(_blackBoardPieces[piece.TileID].Pop());
					if (_blackBoardPieces[piece.TileID].Count != 0) {
						Piece blackPiece = _blackBoardPieces[piece.TileID].Peek();
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
			_playgroundBoardPieces[location].Push(piece);
			piece.TileType = TileTypes.PLAYGROUND;
		}
		else {
			_playgroundBoardNode.MoveChild(piece, -1);
			_playgroundBoardPieces[piece.TileID].Pop();
			_playgroundBoardPieces[location].Push(piece);
			var tween = GetTree().CreateTween();
			tween.TweenProperty(piece, "global_position", _playgroundBoardTiles[location].GlobalPosition + _iconOffset, 0.5);
		}
		
		piece.TileID = location;
	}


	public void AddPiece(int type, int location)
	{
		Piece piece = (Piece)_pieceScene.Instantiate();
		_playgroundBoardNode.AddChild(piece);
		piece.Type = type;
		piece.TileType = TileTypes.PLAYGROUND;
		piece.SetTexture(GD.Load<Texture2D>(_assetPaths[type]));
		piece.GlobalPosition = _playgroundBoardTiles[location].GlobalPosition + _iconOffset;
		_playgroundBoardPieces[location].Push(piece);
		piece.TileID = location;
		piece.PieceSelected += OnPieceSelected;
	}

	private void OnPieceSelected(Piece piece)
	{
		if (_pieceSelected == null)
		{
			_pieceSelected = piece;
			return;
		}
		
		if (_pieceSelected.TileID == piece.TileID && _pieceSelected.TileType == piece.TileType) {
			_pieceSelected = null;
			return;
		}
		
		if (piece.TileType != TileTypes.PLAYGROUND) {
			_pieceSelected = null;
			return;
		}
		
		OnTileClicked(_playgroundBoardTiles[piece.TileID]);
	}

	private void OnButtonPressed()
	{
		AddPiece((int)PieceTypes.BLACK_100PX, 0);
		//addPiece((int)PieceTypes.WHITE_M, 5);
		//addPiece((int)PieceTypes.WHITE_S, 10);
		//addPiece((int)PieceTypes.WHITE_T, 15);

		//int bitmap = 1023;
		//for (int i = 0 ; i < 16; i++) {
		//if ((bitmap & 1) != 0) {
		//_gridArray[15-i].setFilter(SlotStates.FREE);
		//}
		//bitmap = bitmap >> 1;
		//}
	}
}
