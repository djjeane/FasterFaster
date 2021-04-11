using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    void Start()
    {
    }

    
    void Update()
    {
        var curentState = EventsManager.currentState;
        
        //if the game is currently accepting input, lets get all the input needed for the user

        if (curentState == GameState.PlayerMovementInput)
        {

        }
    }
}
