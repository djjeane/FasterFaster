using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using static GameTiles;
using Assets.Scripts;

public class PlayerMovement : MonoBehaviour
{
    public int MoveDistance = 3;
    private float movementSpeed = 10f;
    private Stack<WorldTile> squaresToTravelTo;
    private WorldTile destinationTile;

    private List<WorldTile> cellsInPlayerMoveRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = EventsManager.currentState;

        if(currentState == GameState.PlayerMovementInput)
        {
            var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

            if (Input.GetMouseButtonDown(0))
            {
                SetDestinationTile(playerPos);
            }
            //if they push the confirn button and they have chosen a tile, advance the game state
            if(Input.GetKeyDown(KeyCode.F) && destinationTile != null)
            {
                EventsManager.AdvanceState(); 
            }
        }
        if(currentState == GameState.PlayerMovementOutput)
        {
            MoveIfNeeded();
        }
    }

    private void MoveIfNeeded()
    {
        if (destinationTile != null)
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
        transform.position = Vector3.MoveTowards(transform.position, worldLocation, movementSpeed * Time.deltaTime);
    }

    private void SetDestinationTile(Vector3Int playerPos)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

        cellsInPlayerMoveRadius = GetCellsInMoveRadius(playerPos);
        
        WorldTile worldTile;
        //if we clicked on a tile
        if (tiles.TryGetValue(worldPoint, out worldTile))
        {
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
    private List<WorldTile> GetCellsInMoveRadius(Vector3Int playerPos)
    {
        List<WorldTile> NeighborEmptyTiles = new List<WorldTile>();
        for (int x = playerPos.x - MoveDistance; x <= playerPos.x + MoveDistance; x++)
        {
            for (int y = playerPos.y - MoveDistance; y <= playerPos.y + MoveDistance; y++)
            {

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
                }
            }
        }

        return NeighborEmptyTiles;
    }

}
