using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static GameTiles;

public class PathFinding
{
    // Start is called before the first frame update
    public static Stack<WorldTile> FindPath(Vector3 start, Vector3 end)
    {
        var nodes = PathGrid.nodes;
        Node startNode = nodes[start];
        Node endNode = nodes[end];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            var neighbors = PathGrid.GetNeighbours(node);
            foreach (Node neighbour in neighbors)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = node;
                    node.child = neighbour;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }


        }
        return new Stack<WorldTile>();

    }

    private static Stack<WorldTile> RetracePath(Node startNode, Node endNode)
    {
        Stack<WorldTile> path = new Stack<WorldTile>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            WorldTile currentTile;
            var nodeVector = new Vector3(currentNode.gridX, currentNode.gridY, 0);
            if(GameTiles.tiles.TryGetValue(nodeVector, out currentTile))
            {
                path.Push(currentTile);
                currentNode = currentNode.parent;
            }
            else
            {
                Debug.LogError($"Could not convert node to tile for {nodeVector}");
            }
        }
        //path.Reverse();

        //draw the path in green
        //foreach(var node in path)
        //{
        //    var location = new Vector3(node.gridX, node.gridY, 0);
        //    if (instance.tiles.ContainsKey(location))
        //    {
        //        instance.tiles[location].TilemapMember.SetTileFlags(instance.tiles[location].LocalPlace, TileFlags.None);
        //        instance.tiles[location].TilemapMember.SetColor(instance.tiles[location].LocalPlace, Color.green);
        //    }
        //}

        return path;

    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
public static class PathGrid
{
    public static Dictionary<Vector3, Node> nodes;
    public static void BuildGrid()
    {

        nodes = new Dictionary<Vector3, Node>();
        foreach (var keyValue in GameTiles.tiles)
        {
            var pos = keyValue.Key;
            var tile = keyValue.Value;

            nodes.Add(pos, new Node(tile.entity == null, tile.LocalPlace.x, tile.LocalPlace.y));
        }
    }

    public static List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {

                if (x == 0 && y == 0)
                {
                    //This is the cell the entity is standing on, ignore it
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                var location = new Vector3(checkX, checkY, 0);
                if (tiles.ContainsKey(location))
                {
                    //instance.tiles[location].TilemapMember.SetTileFlags(instance.tiles[location].LocalPlace, TileFlags.None);
                    //instance.tiles[location].TilemapMember.SetColor(instance.tiles[location].LocalPlace, Color.red);
                    neighbours.Add(nodes[location]);
                }

            }
        }
        return neighbours;
    }
}

public class Node
{
    public bool walkable { get; set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public Node parent { get; set; }

    public Node child { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }

    public int fCost => gCost + hCost;

    public Node(bool _walkable, int _gridX, int _gridY)
    {
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }
}

