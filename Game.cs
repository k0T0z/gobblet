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
	private List<Piece> _playgroundBoardPieces;
	private List<string> _assetPaths;

	private List<Tile> _whiteBoardTiles;
	private List<Tile> _blackBoardTiles;
	private List<Piece> _whiteBoardPieces;
	private List<Piece> _blackBoardPieces;

	private PackedScene _whiteBoardScene;
	private PackedScene _blackBoardScene;
	private List<Stack<Piece>> _whiteOutPieces;
	private List<Stack<Piece>> _blackOutPieces;

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
		_playgroundBoardPieces = new List<Piece>();
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
			_playgroundBoardPieces.Add(null); // Initialize the `_pieceArray`.
		}

		_whiteBoardTilesNode = GetNode<VBoxContainer>("WhiteBoard/Board/BoardVBox");
		_blackBoardTilesNode = GetNode<VBoxContainer>("BlackBoard/Board/BoardVBox");
		_whiteBoardNode = GetNode<Control>("WhiteBoard/Board");
		_blackBoardNode = GetNode<Control>("BlackBoard/Board");

		_whiteBoardTiles = new List<Tile>();
		_blackBoardTiles = new List<Tile>();
		_whiteBoardPieces = new List<Piece>();
		_blackBoardPieces = new List<Piece>();

		_whiteOutPieces = new List<Stack<Piece>>();
		_blackOutPieces = new List<Stack<Piece>>();

		for (int i = 0; i < 3; i++)
		{
			_whiteOutPieces.Add(new Stack<Piece>());
			_blackOutPieces.Add(new Stack<Piece>());
			CreateWhiteTile(i);
			CreateBlackTile(i);
			_whiteBoardPieces.Add(null);
			_blackBoardPieces.Add(null);
		}

		InitWhiteOutPieces();
		InitBlackOutPieces();
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

	private void InitWhiteOutPieces()
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
				_whiteOutPieces[j].Push(piece);
			}
		}
	}

	private void InitBlackOutPieces()
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
				_blackOutPieces[j].Push(piece);
			}
		}
	}
	
	public void InitGame() {
		for (int i = 0; i < 3; i++)
		{
			Piece whitePiece = _whiteOutPieces[i].Pop();
			Piece blackPiece = _blackOutPieces[i].Pop();
			_whiteBoardNode.AddChild(whitePiece);
			_blackBoardNode.AddChild(blackPiece);
			whitePiece.GlobalPosition = _whiteBoardTiles[i].GlobalPosition + _iconOffset;
			blackPiece.GlobalPosition = _blackBoardTiles[i].GlobalPosition + _iconOffset;
			_whiteBoardPieces[i] = whitePiece;
			_blackBoardPieces[i] = blackPiece;
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
		if (_playgroundBoardPieces[location] != null)
		{
			_playgroundBoardPieces[location].QueueFree();
			_playgroundBoardPieces[location] = null;
		}

		var tween = GetTree().CreateTween();
		tween.TweenProperty(piece, "global_position", _playgroundBoardTiles[location].GlobalPosition + _iconOffset, 0.5);
		
		if (piece.TileType != TileTypes.PLAYGROUND) {
			switch (piece.TileType) {
				case TileTypes.WHITE: {
					if (_whiteOutPieces[piece.TileID].Count != 0) {
						Piece whitePiece = _whiteOutPieces[piece.TileID].Pop();
						_whiteBoardPieces[piece.TileID] = whitePiece;
						_whiteBoardNode.AddChild(whitePiece);
						whitePiece.GlobalPosition = _whiteBoardTiles[piece.TileID].GlobalPosition + _iconOffset;
						_whiteBoardPieces[piece.TileID] = whitePiece;
						whitePiece.TileID = piece.TileID;
						whitePiece.PieceSelected += OnPieceSelected;
					}
					else {
						_whiteBoardPieces[piece.TileID] = null;
					}
					break;
				}
				case TileTypes.BLACK: {
					if (_blackOutPieces[piece.TileID].Count != 0) {
						Piece blackPiece = _blackOutPieces[piece.TileID].Pop();
						_blackBoardPieces[piece.TileID] = blackPiece;
						_blackBoardNode.AddChild(blackPiece);
						blackPiece.GlobalPosition = _blackBoardTiles[piece.TileID].GlobalPosition + _iconOffset;
						_blackBoardPieces[piece.TileID] = blackPiece;
						blackPiece.TileID = piece.TileID;
						blackPiece.PieceSelected += OnPieceSelected;
					}
					else {
						_whiteBoardPieces[piece.TileID] = null;
					}
					break;
				}
				default: break;
			}
			_playgroundBoardPieces[location] = piece;
			piece.TileType = TileTypes.PLAYGROUND;
		}
		else {
			_playgroundBoardPieces[piece.TileID] = null;
			_playgroundBoardPieces[location] = piece;
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
		_playgroundBoardPieces[location] = piece;
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
