using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{ 
    private AIMovementBase movementType;
    public int turnMoveInterval = 1;
    private int turnsSinceMove = 1;

    private bool hasMovedThisTurn = false;

    private Vector3Int currentDestinationTile;
    // Start is called before the first frame update
    void Start()
    {
        //MovementTypes type = (MovementTypes)Random.Range(0, 4);
        var type = MovementTypes.Brave;
        movementType = AIMovementTypeFactory.GetAIMovementType(type);
        movementType.Init(this.gameObject);
    }

    // Update is called once per frame
    void Update()   
    {
        if(EventsManager.currentState == GameState.EnemyMovement)
        {

            if(currentDestinationTile != Vector3Int.zero)
                movementType.DoMovement(transform.position);

        }
        else
        {
            currentDestinationTile = Vector3Int.zero;
            if (hasMovedThisTurn)
            {
                turnsSinceMove++;
                hasMovedThisTurn = false;
            }
        }
    }

    public Vector3Int GetDestinationMoveTileOfEnemy(List<Vector3Int> deniedDestinations)
    {
        if(movementType != null && turnsSinceMove == turnMoveInterval)
        {
            var currentPosition = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            turnsSinceMove = 0;
            hasMovedThisTurn = true;
            var destinationTile = movementType.GetAndSetDestinationTile(currentPosition, deniedDestinations);
            if(destinationTile != null)
            {
                return destinationTile.LocalPlace;
            }
            else
            {
                //We shouldnt or cant move, so stay in the same place
                return Vector3Int.zero;
            }
        }
        return Vector3Int.zero;
    }

    public void SetDestination(Vector3Int destination)
    {
        currentDestinationTile = destination;
    }


}

public enum MovementTypes
{
    Coward,
    Brave,
    Parity,
    Protector,
    Random
}
