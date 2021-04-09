using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnvironmentBuilder : ScriptableObject
{
    public GameObject wallObject;
    public int numWalls;
    
    // Start is called before the first frame update
    public EnvironmentBuilder(GameObject _wallObject,int _numWalls)
    {

        wallObject = _wallObject;
        numWalls = _numWalls;
    }

    public void SpawnWalls()
    {
        int enemiesSpawned = 0;
        while (enemiesSpawned < numWalls)
        {
            int index = UnityEngine.Random.Range(0, GameTiles.tiles.Count);
            var currentTile = GameTiles.tiles.ElementAt(index).Value;

            if (!currentTile.hasWall)
            {
                var wall = Instantiate(wallObject, currentTile.WorldLocation + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                currentTile.hasWall = true;
                currentTile.entity = wall;
                enemiesSpawned++;
            }
        }
    }

}
