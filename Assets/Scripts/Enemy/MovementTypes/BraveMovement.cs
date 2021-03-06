using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.MovementTypes
{
    public class BraveMovement : AIMovementBase
    {
        internal override WorldTile GetDestinationTile(Vector3Int currentPosition, List<WorldTile> cellsInMoveRadius, List<Vector3Int> deniedDestinations)
        {
            var player = Spawner.Player;
            var playerPos = new Vector3Int(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y), 0);

            //We want to move farther away from the player position
            WorldTile playerTile;
            if (GameTiles.tiles.TryGetValue(playerPos, out playerTile))
            {
                var availableSquares = cellsInMoveRadius.Where(p => !deniedDestinations.Any(p2 => p2 == p.LocalPlace)).ToList();

                WorldTile currentDestination = null;
                float shortestDistanceFromPlayer = Vector3Int.Distance(playerTile.LocalPlace, currentPosition);
                foreach (var availableDestination in availableSquares)
                {
                    var distanceToPlayer = Vector3Int.Distance(playerTile.LocalPlace, availableDestination.LocalPlace);
                    if (distanceToPlayer < shortestDistanceFromPlayer)
                    {
                        currentDestination = availableDestination;
                        shortestDistanceFromPlayer = distanceToPlayer;
                    }
                }

                return currentDestination;
            }
            else
            {
                Console.WriteLine("Could not get the player tile");
            }

            return null;
        }
    }
}
