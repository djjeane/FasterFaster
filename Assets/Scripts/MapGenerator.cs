using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public string seed;
    public bool useRandomSeed;
    public GameObject wallObject;
    public GameObject playerObject;
    public GameObject enemyObject;
    public int numEnemies;
    public int numWalls;

    public Tilemap groundTilemap;
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld(useRandomSeed);
    }

    private void GenerateWorld(bool useRSeed)
    {
        if (useRSeed)
        {
            seed = System.DateTime.Now.ToString();
        }

        GameTiles.BuildTilesDict(groundTilemap);


        Random.InitState(seed.GetHashCode());

        var env = ScriptableObject.CreateInstance<EnvironmentBuilder>();
        env.wallObject = wallObject;
        env.numWalls = numWalls;
        env.SpawnWalls();

        var spawner = ScriptableObject.CreateInstance<Spawner>();
        spawner.playerObject = playerObject;
        spawner.enemyObject = enemyObject;
        spawner.numEnemies = numEnemies;
        spawner.SpawnPlayer();
        spawner.SpawnEnemies();
        PathGrid.BuildGrid();

    }

    // Update is called once per frame
    void Update()
    {
        //On right Click, lets redo the generation with a random seed for testing
        if (Input.GetMouseButtonDown(1))
        {
            foreach (WorldTile tile in GameTiles.tiles.Values)
            {
                if (tile.hasPlayer)
                {
                    Destroy(tile.entity);
                    tile.entity = null;
                    tile.hasPlayer = false;
                }
                if (tile.hasWall)
                {
                    Destroy(tile.entity);
                    tile.entity = null;
                    tile.hasWall = false;
                }
                if (tile.hasEnemy)
                {
                    Destroy(tile.entity);
                    tile.enemyEntity = null;
                    tile.hasEnemy = false;
                }
                if(tile.highLightEntity != null)
                {
                    Destroy(tile.highLightEntity);
                    tile.highLightEntity = null;
                }

            }
            GameTiles.tiles = null;
            GenerateWorld(true);

        }
    }
}
