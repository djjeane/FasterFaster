using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : ScriptableObject
{

    public GameObject playerObject;
    public GameObject enemyObject;
    public string seed;
    public int numEnemies;
    public bool useRandomSeed;

    public Spawner(GameObject playerObject, GameObject enemyObject, int numEnemies)
    {
        this.playerObject = playerObject;
        this.enemyObject = enemyObject;
        this.numEnemies = numEnemies;
    }

    // Start is called before the first frame update


    public void SpawnEnemies()
    {
        int enemiesSpawned = 0;
        while(enemiesSpawned < numEnemies)
        {
            int index = UnityEngine.Random.Range(0, GameTiles.instance.tiles.Count);
            KeyValuePair<Vector3, WorldTile> enemyTile = GameTiles.instance.tiles.ElementAt(index);

            if(!GameTiles.instance.tiles[enemyTile.Key].hasPlayer && !GameTiles.instance.tiles[enemyTile.Key].hasWall && !GameTiles.instance.tiles[enemyTile.Key].hasEnemy)
            {
                var enemy = Instantiate(enemyObject, enemyTile.Key + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                GameTiles.instance.tiles[enemyTile.Key].hasEnemy = true;
                GameTiles.instance.tiles[enemyTile.Key].entity = enemy;
                enemiesSpawned++;
            }
        }
    }

    public void SpawnPlayer()
    {
        WorldTile tile;
        bool spawnedPlayer = false;
        while(!spawnedPlayer)
        {
            int index = UnityEngine.Random.Range(0, GameTiles.instance.tiles.Count);
            KeyValuePair<Vector3, WorldTile> playerTile = GameTiles.instance.tiles.ElementAt(index);
            tile = GameTiles.instance.tiles[playerTile.Key];
            if (!tile.hasWall)
            {
                var player = Instantiate(playerObject, playerTile.Key + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                GameTiles.instance.tiles[playerTile.Key].hasPlayer = true;
                GameTiles.instance.tiles[playerTile.Key].entity = player;
                spawnedPlayer = true;
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
