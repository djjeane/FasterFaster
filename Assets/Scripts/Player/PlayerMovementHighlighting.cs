using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts;
using System;

public class PlayerMovementHighlighting : MonoBehaviour
{
    public int MoveDistance = 3;
    public GameObject highlightPrefab;
    public GameObject highlightHoverPrefab;
    public GameObject highlightClickPrefab;
    private List<WorldTile> cellsInPlayerMoveRadius;
    private Vector3Int lastPlayerPosition;
    private Vector3 lastHoveredCell;
    private Camera mainCamera;
    private Vector3 clickedCell = Vector3.zero;
    private List<WorldTile> highlightedPathToClickedCell;
    private List<WorldTile> highlightedPathToHoveredCell;

    void Start()
    {
        mainCamera = Camera.main;
        highlightedPathToClickedCell = new List<WorldTile>();
        highlightedPathToHoveredCell = new List<WorldTile>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = EventsManager.currentState;
        if(currentState == GameState.PlayerMovementInput )
        {
            var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            HandleCellHighlighting(playerPos);
        }
        else
        {
            UnHighlightCellsInOldMoveRange(new List<WorldTile>());
            UnHighlightOldHoveredCells();
            UnHighlightCellsToOldClickedPosition();
            lastPlayerPosition = Vector3Int.zero;
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
                if (GameTiles.tiles.TryGetValue(cellPostion, out currentCell))
                {
                    if (currentCell.isWalkable)
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

    private void HandleCellHighlighting(Vector3Int playerPos)
    {
        var cellsPlayerCanMoveTo = GetCellsInMoveRadius(playerPos);
        Vector3 point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var currentlyHoveredCell = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        //If the player moved change the moveRange cells

        //If we clicked, lets add some green squares to the path and persist them;
        if(Input.GetMouseButtonDown(0))
        {
            if(isWalkableCellBeingHovered(cellsPlayerCanMoveTo, currentlyHoveredCell))
            {
                //If we have an old path
                if (clickedCell != Vector3.zero)
                {
                    UnHighlightCellsToOldClickedPosition();
                }

                clickedCell = currentlyHoveredCell;
                HighlightCellsToClickedPosition(playerPos, cellsInPlayerMoveRadius, currentlyHoveredCell);
            }
        }

        if (lastPlayerPosition != playerPos)
        {
            UnHighlightCellsInOldMoveRange(cellsPlayerCanMoveTo);
            cellsInPlayerMoveRadius = cellsPlayerCanMoveTo;
            highlightCellsInMoveRange(cellsPlayerCanMoveTo);
            lastPlayerPosition = playerPos;
        }
        else if (isWalkableCellBeingHovered(cellsPlayerCanMoveTo, currentlyHoveredCell))
        {
            //if we are not hovering over a cell that we have clicked already
            if(currentlyHoveredCell != clickedCell)
            {
                //if the hovered cell has changed and isnt null
                if (lastHoveredCell != null && lastHoveredCell != currentlyHoveredCell)
                {
                    UnHighlightOldHoveredCells();
                    HighlightHoveredCells(playerPos, cellsInPlayerMoveRadius, currentlyHoveredCell);
                }

                lastHoveredCell = currentlyHoveredCell;
            }

        }
    }

    private void HighlightCellsToClickedPosition(Vector3Int playerPos, List<WorldTile> cellsInMove, Vector3Int hoveredPosition)
    {
        var cellsBeingHovered = GetPathToHoveredCell(playerPos, cellsInMove, hoveredPosition);
        foreach (var tile in cellsBeingHovered)
        {
            if (tile.highlightMoveClickEntity == null)
            {
                var highlightObj = Instantiate(highlightClickPrefab, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                tile.highlightMoveClickEntity = highlightObj;
                highlightedPathToClickedCell.Add(tile);
            }


        }
    }

    private void UnHighlightCellsToOldClickedPosition()
    {
        foreach(var tile in highlightedPathToClickedCell)
        {
            Destroy(tile.highlightMoveClickEntity);
            tile.highlightMoveClickEntity = null;
        }
        highlightedPathToClickedCell = new List<WorldTile>();
    }

    private void UnHighlightOldHoveredCells()
    {
        foreach (var tile in highlightedPathToHoveredCell)
        {
            if (tile.highlightMoveHoverEntity != null)
            {
                Destroy(tile.highlightMoveHoverEntity);
                tile.highlightMoveHoverEntity = null;
            }
        }
        highlightedPathToHoveredCell = new List<WorldTile>();
    }

    private void HighlightHoveredCells(Vector3Int playerPos, List<WorldTile> cellsInMove, Vector3 hoveredPosition)
    {

        var cellsBeingHovered = GetPathToHoveredCell(playerPos, cellsInMove, hoveredPosition);
        foreach (var tile in cellsBeingHovered)
        {
            if (tile.highlightMoveHoverEntity == null)
            {
                if(tile.WorldLocation.x == hoveredPosition.x && tile.WorldLocation.y == hoveredPosition.y)
                {

                }
                else
                {

                }
                var highlightObj = Instantiate(highlightHoverPrefab, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                tile.highlightMoveHoverEntity = highlightObj;
                highlightedPathToHoveredCell.Add(tile);
            }

        }
    }

    private void UnHighlightCellsInOldMoveRange(List<WorldTile> cellsPlayerCanMoveTo)
    {
        foreach (var tile in GameTiles.tiles.Values)
        {
            if (tile.highlightMoveRangeEntity != null)
            {
                if (!cellsPlayerCanMoveTo.Contains(tile))
                {
                    Destroy(tile.highlightMoveRangeEntity);
                }
            }
        }
    }

    private void highlightCellsInMoveRange(List<WorldTile> cellsInMove)
    {
        foreach (var tile in cellsInMove)
        {
            if (tile.highlightMoveRangeEntity == null)
            {
                var highlightObj = Instantiate(highlightPrefab, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                tile.highlightMoveRangeEntity = highlightObj;
            }
        }
    }

    private List<WorldTile> GetPathToHoveredCell(Vector3Int playerPos, List<WorldTile> cellsInMove, Vector3 hoveredPosition)
    {
        WorldTile targetTile;

        if (isWalkableCellBeingHovered(cellsInMove, hoveredPosition))
        {
            targetTile = cellsInMove.First(x => x.WorldLocation.x == hoveredPosition.x && x.WorldLocation.y == hoveredPosition.y);

            var pathToSquare = PathFinding.FindPath(playerPos, targetTile.WorldLocation);
            return pathToSquare.ToList();
        }
        return new List<WorldTile>();
    }

    private bool isWalkableCellBeingHovered(List<WorldTile> cellsInMove, Vector3 hoveredPosition)
    {

        if (cellsInMove.Any(x => x.WorldLocation.x == hoveredPosition.x && x.WorldLocation.y == hoveredPosition.y))
        {
            return true;
        }
        return false;
    }

}
