using Assets.Scripts;
using Assets.Scripts.Enemy.MovementTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMovementBase
{
    public int MoveDistance = 3;
    private float movementSpeed = 10f;
    private Stack<WorldTile> squaresToTravelTo;
    private WorldTile destinationTile;
    public GameObject enemyEntity;

    private List<WorldTile> cellsInMoveRadius;
    public void Init(GameObject entity)
    {
        enemyEntity = entity;
    }
    public  WorldTile GetAndSetDestinationTile(Vector3Int currentPosition, List<Vector3Int> deniedDestinations)
    {
        cellsInMoveRadius = GetCellsInMoveRadius(currentPosition);

        WorldTile destinationTile = GetDestinationTile(cellsInMoveRadius, deniedDestinations);
        //if we clicked on a tile
        if (destinationTile != null)
        {
            WorldTile enemyTile;
            if (GameTiles.tiles.TryGetValue(currentPosition, out enemyTile))
            {
                enemyTile.hasEnemy = false;
                enemyTile.enemyEntity = null;
            }

            //lets chart a destination path to the target square
            squaresToTravelTo = PathFinding.FindPath(currentPosition, destinationTile.WorldLocation);

            //if we can get there, grab the first step
            if (squaresToTravelTo.Count > 0)
            {
                this.destinationTile = squaresToTravelTo.Pop();
            }
        }

        return destinationTile;
    }

    internal void DoMovement(Vector3 currentPosition)
    {
        if (destinationTile != null)
        {
            var destinationVector3 =  destinationTile.LocalPlace + new Vector3(0.5f, 0.5f, 0);
            if (Vector3.Distance(destinationVector3, currentPosition) > 0.1f)
            {
                MoveEntity(destinationVector3);
            }
            else
            {
                if (squaresToTravelTo.Count != 0)
                {
                    destinationTile = squaresToTravelTo.Pop();
                }
                else
                {
                    destinationTile.hasEnemy = true;
                    destinationTile.enemyEntity = enemyEntity;
                    destinationTile = null;
                }

            }
        }
    }

    private void MoveEntity(Vector3 targetLocation)
    {
        enemyEntity.transform.position = Vector3.MoveTowards(enemyEntity.transform.position, targetLocation, movementSpeed * Time.deltaTime);
    }

    private List<WorldTile> GetCellsInMoveRadius(Vector3Int currentPosition)
    {
        List<WorldTile> NeighborEmptyTiles = new List<WorldTile>();
        for (int x = currentPosition.x - MoveDistance; x <= currentPosition.x + MoveDistance; x++)
        {
            for (int y = currentPosition.y - MoveDistance; y <= currentPosition.y + MoveDistance; y++)
            {

                var cellPostion = new Vector3(x, y, 0);
                WorldTile currentCell;
                if (GameTiles.tiles.TryGetValue(cellPostion, out currentCell))
                {
                    if (currentCell.isWalkable)
                    {
                        var pathToSquare = PathFinding.FindPath(currentPosition, currentCell.WorldLocation);
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

    internal abstract WorldTile GetDestinationTile(List<WorldTile> cellsInMoveRadius, List<Vector3Int> deniedDestinations);
}

public static class AIMovementTypeFactory
{
    public static AIMovementBase GetAIMovementType(MovementTypes movementType)
    {
        switch (movementType)
        {
            case MovementTypes.Random:
                return new RandomMovement();
            case MovementTypes.Coward:
                return new CowardMovement();
            default:
                return new CowardMovement();
        }
    }
}
