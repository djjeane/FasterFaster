using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : Attack
{
    public override KeyCode attackKey { get; set; }
    public override int CellAttackRange { get; set; }

    internal override void PerformAttack(Vector3Int locationToAttack, Vector3Int playerPos)
    {
        if (isWithinRange(locationToAttack, playerPos))
        {
            var tiles = GameTiles.tiles; // This is our Dictionary of tiles
            WorldTile tile;
            if (tiles.TryGetValue(locationToAttack, out tile))
            {
                if (tile.hasEnemy)
                {
                    tile.hasEnemy = false;
                    Destroy(tile.enemyEntity);
                    tile.enemyEntity = null;
                }
            }
        }
    }

    private bool isWithinRange(Vector3Int worldPoint,Vector3Int playerPos)
    {
        var playerPosition = new Vector3Int(Mathf.FloorToInt(playerPos.x), Mathf.FloorToInt(playerPos.y), 0);

        for (int x = playerPosition.x - CellAttackRange; x <= playerPosition.x + CellAttackRange; x++)
        {
            for (int y = playerPosition.y - CellAttackRange; y <= playerPosition.y + CellAttackRange; y++)
            {
                if (x == playerPosition.x && y == playerPosition.y)
                {
                    //This is the cell the player is standing on, ignore it
                    continue;
                }

                if (worldPoint.x == x && worldPoint.y == y)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
public abstract class Attack : ScriptableObject
{
    public abstract KeyCode attackKey { get; set; }
    public abstract int CellAttackRange { get; set; }
    public GameObject BulletPrefab { get; internal set; }

    internal void InitMelee(int cellAttackRange)
    {
        attackKey = KeyCode.Space;
        CellAttackRange = cellAttackRange;
    }

    internal void InitRanged(Transform transform)
    {
    }


    internal abstract void PerformAttack(Vector3Int locationToAttack, Vector3Int playerPos);
}
