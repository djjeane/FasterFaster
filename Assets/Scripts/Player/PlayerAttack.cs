using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    public int CellAttackRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y),0);
            
            if (isWithinRange(worldPoint))
            {
                var tiles = GameTiles.tiles; // This is our Dictionary of tiles
                WorldTile tile;
                if (tiles.TryGetValue(worldPoint, out tile))
                { 
                    print("Attacking tile : \n" + tile.ToString());   
                    if (tile.hasEnemy)
                    {
                        tile.hasEnemy = false;
                        Destroy(tile.entity);
                        tile.entity = null;
                    }
                }
            }
        }
    }
    
    public bool isWithinRange(Vector3Int worldPoint)
    {
        var tiles = GameTiles.tiles; // This is our Dictionary of tiles
        var playerPosition  = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);

        for (int x = playerPosition.x - CellAttackRange; x <= playerPosition.x + CellAttackRange; x++)
        {
            for (int y = playerPosition.y - CellAttackRange; y <= playerPosition.y + CellAttackRange; y++)
            {
                if(x == playerPosition.x && y == playerPosition.y)
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
