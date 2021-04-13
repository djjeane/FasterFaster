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

    public static List<GameObject> Enemies;

    public Spawner(GameObject playerObject, GameObject enemyObject, int numEnemies)
    {
        this.playerObject = playerObject;
        this.enemyObject = enemyObject;
        this.numEnemies = numEnemies;
    }

    public void SpawnEnemies()
    {
        int enemiesSpawned = 0;
        Enemies = new List<GameObject>();
        while(enemiesSpawned < numEnemies)
        {
            int index = UnityEngine.Random.Range(0, GameTiles.tiles.Count);
            var tile = GameTiles.tiles.ElementAt(index).Value;
            if (!tile.hasPlayer && !tile.hasWall && !tile.hasEnemy)
            {
                var enemy = Instantiate(enemyObject, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                var movement = enemy.GetComponent<EnemyMovement>();
                Enemies.Add(enemy);
                tile.hasEnemy = true;
                tile.enemyEntity = enemy;
                enemiesSpawned++;
            }
        }
        int I = 0;
    }

    public void SpawnPlayer()
    {
        bool spawnedPlayer = false;
        while(!spawnedPlayer)
        {
            int index = UnityEngine.Random.Range(0, GameTiles.tiles.Count);
            var tile = GameTiles.tiles.ElementAt(index).Value;
            if (!tile.hasWall)
            {
                var player = Instantiate(playerObject, tile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                tile.hasPlayer = true;
                tile.entity = player;
                spawnedPlayer = true;
            }
        }
    }
}
