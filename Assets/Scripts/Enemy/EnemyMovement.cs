using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{ 
    private AIMovementBase movementType;
    public int turnMoveInterval = 0;
    private int turnsSinceMove = 0;

    private bool hasMovedThisTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        //MovementTypes type = (MovementTypes)Random.Range(0, 4);
        var type = MovementTypes.Random;
        movementType = AIMovementTypeFactory.GetAIMovementType(type);
        movementType.Init(this.gameObject);
    }

    // Update is called once per frame
    void Update()   
    {
        if(EventsManager.currentState == GameState.EnemyMovement)
        {
            var currentPosition = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            if (turnsSinceMove == turnMoveInterval && !hasMovedThisTurn)
            {
                movementType.SetDestinationTile(currentPosition);
                turnsSinceMove = 0;
                hasMovedThisTurn = true;
            }
            movementType.DoMovement(transform.position);

        }
        else
        {
            if(hasMovedThisTurn)
            {
                //turnsSinceMove++;
                hasMovedThisTurn = false;
            }
        }
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
