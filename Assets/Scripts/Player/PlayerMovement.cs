using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using static GameTiles;

public class PlayerMovement : MonoBehaviour
{
    public int MoveDistance = 3;
    private float movementSpeed = 10f;
    public GameObject highlightPrefab;
    private Stack<WorldTile> squaresToTravelTo;
    private WorldTile destinationTile;

    private List<WorldTile> cellsInPlayerMoveRadius;
    private Vector3Int lastPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        //Handles the movement highlighting
        var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        if(lastPlayerPosition != playerPos)
        {
            var cellsPlayerCanMoveTo = GetCellsInMoveRadius(playerPos);
            UnHighlightCells(cellsPlayerCanMoveTo);
            cellsInPlayerMoveRadius = cellsPlayerCanMoveTo;
            HighLightEmptyCells();
            lastPlayerPosition = playerPos;
        }
     
        //Handles movement on click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
            WorldTile worldTile;
            
            //if we clicked on a tile
            if (tiles.TryGetValue(worldPoint, out worldTile))
            {
                print(worldTile.ToString());
                //if we clicked on a sqaure that is within our move distance
                if (cellsInPlayerMoveRadius.Contains(worldTile))
                {
                    WorldTile playerTile;
                    if (tiles.TryGetValue(playerPos, out playerTile))
                    {
                        playerTile.hasPlayer = false;
                        playerTile.entity = null;
                    }

                    //lets chart a destination path to the target square
                    squaresToTravelTo = PathFinding.FindPath(playerPos, worldPoint);

                    //if we can get there, grab the first step
                    if (squaresToTravelTo.Count > 0)
                    {
                        destinationTile = squaresToTravelTo.Pop();
                    }

                }
            }
        }


        if(destinationTile != null)
        {
            var tilePosition = new Vector3(destinationTile.LocalPlace.x, destinationTile.LocalPlace.y, 0) + new Vector3(0.5f, 0.5f, 0);
            if (Vector3.Distance(tilePosition, transform.position) > 0.1f)
            {
                MovePlayer(tilePosition);
            }
            else
            {
                if (squaresToTravelTo.Count != 0)
                {
                    destinationTile = squaresToTravelTo.Pop();
                }
                else
                    destinationTile = null;
            }
        }
    }

    private void MovePlayer(Vector3 worldLocation)
    {
        transform.position = Vector3.MoveTowards(transform.position,worldLocation,movementSpeed * Time.deltaTime);

    }
    private void UnHighlightCells(List<WorldTile> cellsPlayerCanMoveTo)
    {
        foreach(var tile in GameTiles.tiles.Values)
        {
            if (tile.highLightEntity != null)
            {
                if (!cellsPlayerCanMoveTo.Contains(tile))
                {
                    Destroy(tile.highLightEntity);
                }
            }
        }
    }

    private void HighLightEmptyCells()
    {
        var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

        foreach (var tile in GetCellsInMoveRadius(playerPos))
        {
            if(tile.highLightEntity == null)
            {
                var highlightObj = Instantiate(highlightPrefab, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                tile.highLightEntity = highlightObj;
            }
            //tile.TilemapMember.SetTileFlags(tile.LocalPlace, TileFlags.None);
            //tile.TilemapMember.SetColor(tile.LocalPlace, Color.green);
        }
    }



    private List<WorldTile> GetCellsInMoveRadius(Vector3Int playerPos)
    {
        List<WorldTile> NeighborEmptyTiles = new List<WorldTile>();
        for (int x = playerPos.x - MoveDistance; x <= playerPos.x + MoveDistance; x++)
        {
            for (int y = playerPos.y - MoveDistance; y <= playerPos.y + MoveDistance; y++)
            {
                //this is our current position, skip it
                //if(x == playerPos.x && y == playerPos.y)
                //{
                //    continue;
                //}

                var cellPostion = new Vector3(x, y, 0);
                WorldTile currentCell;
                if(GameTiles.tiles.TryGetValue(cellPostion, out currentCell))
                {
                    if(currentCell.isWalkable)
                    {
                        var pathToSquare = PathFinding.FindPath(playerPos, currentCell.WorldLocation);
                        if (pathToSquare.Count > 0)
                        {
                            if (pathToSquare.Count <= MoveDistance)
                            {
                                NeighborEmptyTiles.Add(currentCell);
                            }
                        }
                    }
                    else
                    {
                        currentCell.TilemapMember.SetColor(currentCell.LocalPlace, Color.red);
                        print("Could not walk on tile: \n" + currentCell.ToString());
                    }
                }
            }
        }

        return NeighborEmptyTiles;
    }

}
