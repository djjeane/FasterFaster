
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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
        //List<WorldTile> whereEntityNotNull = GameTiles.tiles.Values.Where(x => x.entity != null).ToList();
        //foreach (WorldTile tile in GameTiles.tiles.Values)
        //{
        //    tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
        //    if (tile.entity != null)
        //    {
        //        tile.TilemapMember.SetColor(tile.LocalPlace, Color.black);
        //    }
        //    if (tile.hasPlayer)
        //    {
        //        tile.TilemapMember.SetColor(tile.LocalPlace, Color.white);
        //    }
        //    if (tile.hasWall)
        //    {
        //        tile.TilemapMember.SetColor(tile.LocalPlace, Color.green);
        //    }
        //    if (tile.hasBullet)
        //    {
        //        tile.TilemapMember.SetColor(tile.LocalPlace, Color.blue);
        //    }
        //    if (tile.hasEnemy)
        //    {
        //        tile.TilemapMember.SetColor(tile.LocalPlace, Color.red);
        //    }

            //}
        
    }
}
