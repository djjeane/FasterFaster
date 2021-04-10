using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile
{

    public Vector3Int LocalPlace { get; set; }

    public Vector3 WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public string Name { get; set; }

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public WorldTile ExploredFrom { get; set; }

    public int Cost { get; set; }

    public GameObject entity { get;set; }

    public GameObject enemyEntity { get; set; }
    public GameObject highLightEntity { get; set; }
    public bool hasPlayer { get; set; } = false;

    public bool hasWall { get; set; } = false;

    public bool hasEnemy { get; set; } = false;

    public bool hasBullet { get; set; } = false;

    public bool hasItem { get; set; } = false;

    public bool isWalkable
    {
        get
        {
            return !hasEnemy && !hasWall && !hasPlayer & !hasBullet;
        }
    }
    public override string ToString()
    {
        string str = "";
        str += "Tile : " + Name + "\n";
        str += "Has Player : " + hasPlayer + "\n";
        str += "Has Wall : " + hasWall + "\n";
        str += "Has Enemy : " + hasEnemy + "\n";
        str += "Has Item : " + hasItem + "\n";
        str += "Has Bullet : " + hasBullet + "\n";
        str += "Has Entity : " + entity != null + "\n";

        return str;

    }
}