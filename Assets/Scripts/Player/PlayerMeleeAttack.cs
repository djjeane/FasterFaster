using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : Attack
{
    public override KeyCode attackKey { get; set; }
    public override int CellAttackRange { get; set; }
    public override Transform attackSourceLocation { get; set; }

    internal override void PerformAttack(Vector3Int locationToAttack)
    {
        if (isWithinRange(locationToAttack))
        {
            var tiles = GameTiles.tiles; // This is our Dictionary of tiles
            WorldTile tile;
            if (tiles.TryGetValue(locationToAttack, out tile))
            {
                if (tile.hasEnemy)
                {
                    tile.hasEnemy = false;
                    Destroy(tile.entity);
                    tile.entity = null;
                }
            }
        }
    }

    private bool isWithinRange(Vector3Int worldPoint)
    {
        var playerPosition = new Vector3Int(Mathf.FloorToInt(attackSourceLocation.position.x), Mathf.FloorToInt(attackSourceLocation.position.y), 0);

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
    public abstract Transform attackSourceLocation { get; set; }

    internal void InitMelee(int cellAttackRange, Transform transform)
    {
        attackKey = KeyCode.Space;
        CellAttackRange = cellAttackRange;
        attackSourceLocation = transform;
    }

    internal void InitRanged(Transform transform)
    {
        attackSourceLocation = transform;
    }


    internal abstract void PerformAttack(Vector3Int locationToAttack);
}
