using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public string seed;
    public bool useRandomSeed;
    public GameObject wallObject;
    public GameObject playerObject;
    public GameObject enemyObject;
    public int numEnemies;
    public int numWalls;
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld(useRandomSeed);


    }

    private void GenerateWorld(bool useRSeed)
    {
        if (useRSeed)
        {
            seed = Time.time.ToString();
        }

        Random.InitState(seed.GetHashCode());

        var env = new EnvironmentBuilder(wallObject, numWalls);
        env.SpawnWalls();

        var spawner = new Spawner(playerObject, enemyObject, numEnemies);
        spawner.SpawnPlayer();
        spawner.SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        //On right Click, lets redo the generation with a random seed for testing
        if (Input.GetMouseButtonDown(1))
        {
            foreach (KeyValuePair<Vector3, WorldTile> entry in GameTiles.instance.tiles)
            {
                if (entry.Value.entity != null)
                {
                    Destroy(entry.Value.entity);
                    entry.Value.hasEnemy = false;
                    entry.Value.hasItem = false;
                    entry.Value.hasPlayer = false;
                    entry.Value.hasWall = false;
                }
            }

            GenerateWorld(true);
        }
    }
}
