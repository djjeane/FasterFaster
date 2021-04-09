using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private WorldTile destinationTile;
    private float movementSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

            var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles
            if(destinationTile != null)
            {
                destinationTile.hasPlayer = false;
            }
            if (tiles.TryGetValue(worldPoint, out destinationTile))
            {
                print("Tile " + destinationTile.Name + " costs: " + destinationTile.Cost);
                destinationTile.TilemapMember.SetTileFlags(destinationTile.LocalPlace, TileFlags.None);
            }
        }

        if(destinationTile != null && Vector3.Distance(destinationTile.WorldLocation + new Vector3(0.5f, 0.5f, 0), transform.position) > 0.1f)
        {
            MovePlayer(destinationTile.WorldLocation + new Vector3(0.5f, 0.5f, 0));

        }
    }

    private void MovePlayer(Vector3 worldLocation)
    {
        destinationTile.hasPlayer = true;
        transform.position = Vector3.MoveTowards(transform.position,worldLocation,movementSpeed * Time.deltaTime);

    }
}
