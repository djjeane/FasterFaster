
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestingGameTiles : MonoBehaviour
{
	private WorldTile _tile;


	// Update is called once per frame
	private void Update()
	{
		//if (Input.GetMouseButtonDown(0))
		//{
		//	Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

		//	var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

		//          if (tiles.TryGetValue(worldPoint, out _tile))
		//          {
		//              print("Tile " + _tile.Name + " costs: " + _tile.Cost);
		//              _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
		//              if (_tile.hasPlayer)
		//		{
		//_tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
		//			_tile.TilemapMember.SetColor(_tile.LocalPlace, Color.red);
		//		}
		//          }
		//      }
		//var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

		//foreach (KeyValuePair<Vector3, WorldTile> entry in tiles)
		//{
		//	if (entry.Value.hasPlayer)
		//	{
		//		entry.Value.TilemapMember.SetColor(entry.Value.LocalPlace, Color.red);
		//	}
		//	else
		//          {
		//		entry.Value.TilemapMember.SetColor(entry.Value.LocalPlace, Color.clear);

		//	}

		//}
	}
}
