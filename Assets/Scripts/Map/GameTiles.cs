using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameTiles
{
	public static Tilemap groundTileMap;

	public static Dictionary<Vector3, WorldTile> tiles;

	// Use this for initialization
	public static void BuildTilesDict(Tilemap tilemap)
	{
		groundTileMap = tilemap;
		tiles = new Dictionary<Vector3, WorldTile>();
		foreach (Vector3Int pos in groundTileMap.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, 0);

			if (!groundTileMap.HasTile(localPlace)) continue;
			var tile = new WorldTile
			{
				LocalPlace = localPlace,
				WorldLocation = groundTileMap.CellToWorld(localPlace),
				TileBase = groundTileMap.GetTile(localPlace),
				TilemapMember = groundTileMap,
				Name = localPlace.x + "," + localPlace.y,
				Cost = 1
			};

			tiles.Add(tile.WorldLocation, tile);
		}
	}
}
