using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : Attack
{
    public override KeyCode attackKey { get; set; } = KeyCode.E;
    public override int CellAttackRange { get; set ; }

    public PlayerRangedAttack()
    {
        attackKey = KeyCode.E;
    }

    internal override void PerformAttack(Vector3Int locationToAttack, Vector3Int playerPos)
    {
        var playerPosition = new Vector3Int(Mathf.FloorToInt(playerPos.x), Mathf.FloorToInt(playerPos.y), 0);

        //We want to know what direction from the player was selected to shoot our bullet in
        //e
        Vector3 bullChangeDirFromPlayer = GetNormalizedBulletDirection(playerPosition, locationToAttack);
        if(!(bullChangeDirFromPlayer.x == 0 && bullChangeDirFromPlayer.y == 0))
        {
            Vector3 BulletLocation = playerPosition + bullChangeDirFromPlayer;
            WorldTile tile;
            if (GameTiles.tiles.TryGetValue(BulletLocation, out tile))
            {
                if(!tile.hasBullet && !tile.hasEnemy && !tile.hasPlayer && !tile.hasWall)
                {
                    var bullet = Instantiate(BulletPrefab, BulletLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    var bulletClassRef = bullet.GetComponent<Bullet>();
                    tile.hasBullet = true;
                    tile.entity = bullet;
                    bulletClassRef.InitBullet(bullChangeDirFromPlayer);
                }

            }
        }
    }

    private Vector3Int GetNormalizedBulletDirection(Vector3Int playerPosition, Vector3Int locationToAttack)
    {
        Vector3Int newVector;
        int changeInX;
        int changeInY;
        if (playerPosition.x > locationToAttack.x)
        {
            changeInX = -1;
        }
        else if (playerPosition.x == locationToAttack.x)
        {
            changeInX = 0;
        }
        else
        {
            changeInX = 1;
        }

        if (playerPosition.y > locationToAttack.y)
        {
            changeInY = -1;
        }
        else if (playerPosition.y == locationToAttack.y)
        {
            changeInY = 0;
        }
        else
        {
            changeInY = 1;
        }

        newVector = new Vector3Int(changeInX, changeInY,0);

        return newVector;
    }
}
