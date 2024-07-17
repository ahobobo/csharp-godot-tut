using Godot;
using System;
using System.Collections.Generic;
public partial class Grid : Node2D
{
	private Tile[,] grid;
	private PackedScene sceneTile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneTile = ResourceLoader.Load<PackedScene>("res://Tile.tscn");

		grid = new Tile[4, 4];

		PopulateStartingTiles();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("up"))
		{
			MoveTiles("up");
		}
		if (@event.IsActionPressed("down"))
		{
			MoveTiles("down");
		}
		if (@event.IsActionPressed("left"))
		{
			MoveTiles("left");
		}
		if (@event.IsActionPressed("right"))
		{
			MoveTiles("right");
		}
    }
	/// <summary>
	/// looking at the grid and adding all the grid tiles to the stack
	/// we clear the grid as we go.
	/// after we have all the grid tiles in the stack, we move them based on the dir of movement.
	/// </summary>
    private bool MoveTiles(String direction) 
	{
		GD.Print("MoveTiles() called." + direction);
		bool movementOccurred = false;

		bool isHorizontal = direction == "left" || direction == "right";
		bool isReverse = direction == "up" || direction == "left";

		for (int i = 0; i < 4; i++) {
			var titles = new Stack<Tile>();

			for (int j = 0; j < 4; j++) {
				int x = isHorizontal ? (isReverse ? 3 - j : j) : i;
				int y = isHorizontal ? i : (isReverse ? 3 - j : j);

				if (grid[x, y] != null) 
				{
					titles.Push(grid[x, y]);
					grid[x, y] = null; 
				}
			}

			int newIndex = isReverse ? 0 : 3;

			while(titles.Count > 0)
			{
				var current = titles.Pop();
				var next = titles.Count > 0 ? titles.Peek() : null;
				Tile merged = null;

				// Check for merges. 
				if (next != null && current.GetValue() == next.GetValue())
				{
					merged = titles.Pop();
					current.SetValue(current.GetValue() * 2);
				} 
			}
		}

		return movementOccurred;
	}

	private Vector2 ArrayToTileCoords(Vector2 arrayCoords)
	{
		return new Vector2(arrayCoords.X * 115 + 15, arrayCoords.Y * 115 + 15);
	}

	private void PopulateStartingTiles() 
	{
		var rand = new Random();

		var tile1Coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
		var tile2Coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));

		while (tile1Coords.X == tile2Coords.X && tile1Coords.Y == tile2Coords.Y)
		{
			tile1Coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
			tile2Coords = new Vector2(rand.Next(0, 4), rand.Next(0, 4));
		}

		Tile t1 = sceneTile.Instantiate<Tile>();
		t1.Position = ArrayToTileCoords(tile1Coords);
		t1.SetValue(2);
		AddChild(t1);
		
		Tile t2 = sceneTile.Instantiate<Tile>();
		t2.Position = ArrayToTileCoords(tile2Coords);
		t2.SetValue(2);
		AddChild(t2);

		grid[(int) tile1Coords.X, (int) tile1Coords.Y] = t1;
		grid[(int) tile2Coords.X, (int) tile2Coords.Y] = t2;
	}
}
