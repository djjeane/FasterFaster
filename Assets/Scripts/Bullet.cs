using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts;

public class Bullet : MonoBehaviour
{
    private Vector3Int playerPosition;
    private int cellsToTravelEachRound;

    public float bulletSpeed;
    private Vector3 bulletVelocity;
    private Vector3 nextBulPostion;
    private Vector3 startingBulletPosition;
    public int tilesPerRound = 1;
    private int tilesMovedThisRound = 0;
    // Start is called before the first frame update
    void Start()
    {
        startingBulletPosition = new Vector3(transform.position.x,transform.position.y, 0);
        //spawns in the neighboring cells
    }
    public void InitBullet(Vector3 bulletVector)
    {
        bulletVelocity = bulletVector;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventsManager.currentState == GameState.BulletMovement)
        {
            var currentBulletPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            WorldTile currentBulletTile;
            if (!GameTiles.tiles.TryGetValue(currentBulletPos, out currentBulletTile))
            {
                Destroy(gameObject);
            }
            MoveBullet();
        }
        else
        {
            tilesMovedThisRound = 0;
        }
      
    }
    private void RemoveBulletFromTile(Vector3 location)
    {
        WorldTile tile;
        var tileLocation = new Vector3Int(Mathf.FloorToInt(location.x), Mathf.FloorToInt(location.y), 0);
        if (GameTiles.tiles.TryGetValue(tileLocation, out tile))
        {
            tile.hasBullet = false;
            tile.entity = null;
        }
    }

    private void AddBulletToTile(Vector3 location)
    {
        WorldTile tile;
        var tileLocation = new Vector3Int(Mathf.FloorToInt(location.x), Mathf.FloorToInt(location.y), 0);
        if (GameTiles.tiles.TryGetValue(tileLocation, out tile))
        {
            tile.hasBullet = true;
            tile.entity = gameObject;
        }
    }

    private void MoveBullet()
    {
        
        if (bulletVelocity != null)
        {
            if(nextBulPostion == Vector3.zero)
            {
                nextBulPostion = startingBulletPosition + bulletVelocity;
                RemoveBulletFromTile(startingBulletPosition);
            }
            else
            {
                CheckForCollisions();
                //once we start moving
                //if we get to where we are going, incriment the next bullet position
                if (Vector3.Distance(transform.position,nextBulPostion) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextBulPostion, bulletSpeed * Time.deltaTime);
                }
                else
                {
                    tilesMovedThisRound++;
                    if(tilesMovedThisRound <= tilesPerRound)
                    {
                        RemoveBulletFromTile(nextBulPostion);
                        nextBulPostion = nextBulPostion + bulletVelocity;
                        AddBulletToTile(nextBulPostion);
                        CheckForCollisions();
                    }
                }
            }
        }
    }

    private void CheckForCollisions()
    {
        WorldTile tile;
        var tileLocation = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        if (GameTiles.tiles.TryGetValue(tileLocation, out tile))
        {
            if (tile.hasWall)
            {
                print("Bullet hit wall " + tile.ToString());
                tile.hasBullet = false;
                Destroy(gameObject);
            }
            if (tile.hasEnemy)
            {
                RemoveBulletFromTile(nextBulPostion);
                tile.hasBullet = false;
                tile.hasEnemy = false;
                print("Bullet hit enemy " + tile.ToString());
                Destroy(tile.enemyEntity);
                Destroy(gameObject);
            }
            
        }
    }
}
