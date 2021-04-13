using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.MovementTypes
{
    class RandomMovement : AIMovementBase
    {
        internal override WorldTile GetDestinationTile(List<WorldTile> cellsInMoveRadius, List<Vector3Int> deniedDestinations)
        {
            //Remove the denied destinations from the cellsInMoveRadius
            var availableSquares = cellsInMoveRadius.Where(p => !deniedDestinations.Any(p2 => p2 == p.LocalPlace)).ToList();
            if(availableSquares.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, availableSquares.Count);
                var tile = cellsInMoveRadius.ElementAt(index);
                return tile;
            }
            else
            {
                return null;
            }
        }
    }
}
