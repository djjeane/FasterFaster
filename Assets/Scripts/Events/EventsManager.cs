using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EventsManager : MonoBehaviour
    {
        public Text CurrentEventUI;
        public Text timer;

        [Range(5, 15)]
        public int EventDurationInSecs = 10;

        public static EventsManager instance;
        private bool advanceState = false;
        public static GameState currentState = GameState.PlayerMovementInput;
        private GameEvent currentEvent;
        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            currentEvent = GameEventFactory.GetGameEvent(currentState);
            currentEvent.InitEvent(EventDurationInSecs, CurrentEventUI, timer);

        }

        // Update is called once per frame
        void Update()
        {

            if (advanceState)
            {
                currentEvent.CloseEvent();
                switch (currentState)
                {
                    case GameState.PlayerMovementInput:
                        currentState = GameState.PlayerMovementOutput;
                        break;
                    case GameState.PlayerMovementOutput:
                        currentState = GameState.PlayerAttackInput;
                        break;
                    case GameState.PlayerAttackInput:
                        currentState = GameState.BulletMovement;
                        break;
                    case GameState.BulletMovement:
                        currentState = GameState.EnemyMovement;
                        break;
                    case GameState.EnemyMovement:
                        currentState = GameState.EnemiesFire;
                        break;
                    case GameState.EnemiesFire:
                        currentState = GameState.PlayerMovementInput;
                        break;
                }
                advanceState = false;
                currentEvent = GameEventFactory.GetGameEvent(currentState);

                currentEvent.InitEvent(EventDurationInSecs,CurrentEventUI, timer);
            }

            currentEvent.FireEvent();
            advanceState = currentEvent.ShouldAdvanceState();
            if (Input.GetKeyDown(KeyCode.F))
            {
                advanceState = true;
            }

        }

        internal static void AdvanceState()
        {
            EventsManager.instance.advanceState = true;
        }

    }
    public enum GameState
    {
        PlayerMovementInput,
        PlayerMovementOutput,
        PlayerAttackInput,
        BulletMovement,
        EnemyMovement,
        EnemiesFire,
    }
}