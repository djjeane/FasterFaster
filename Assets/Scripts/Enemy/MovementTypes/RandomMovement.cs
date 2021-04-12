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
        internal override WorldTile GetDestinationTile(List<WorldTile> cellsInMoveRadius)
        {
            int index = UnityEngine.Random.Range(0, cellsInMoveRadius.Count);
            var tile = GameTiles.tiles.ElementAt(index).Value;
            return tile;
        }
    }
}
