using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.MovementTypes
{
    public class CowardMovement : AIMovementBase
    {

        internal override WorldTile GetDestinationTile(List<WorldTile> cellsInMoveRadius, List<Vector3Int> deniedDestinations)
        {
            throw new NotImplementedException();
        }
    }
}
