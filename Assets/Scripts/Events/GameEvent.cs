using System;
using UnityEngine.UI;
using UnityEngine;


namespace Assets.Scripts
{
    public class PlayerInputEvent : GameEvent
    {
   
        internal override string ToString()
        {
            return "Player Input";
        }

    }

    internal class EnemiesFireEvent : GameEvent
    {

        internal override string ToString()
        {
            return "Enemies Fire";
        }
    }

    internal class EnemyMovementEvent : GameEvent
    {

        internal override string ToString()
        {
            return "Enemies Move";
        }
    }

    internal class BulletMovementEvent : GameEvent
    {

        internal override string ToString()
        {
            return "Bullets Move";
        }
    }

    internal class PlayerOutputEvent : GameEvent
    {

        internal override string ToString()
        {
           return "Player Output";
        }
    }

    internal class PlayerAttackInput : GameEvent
    {

        internal override string ToString()
        {
            return "Player Attack Input";
        }
    }

    public abstract class GameEvent
    {
        protected Text currentEventUI;
        protected Text timerUI;
        protected int eventDurationInSecs;

        private float eventStartTime;
        internal void FireEvent()
        {
            currentEventUI.text = this.ToString(); 
            if(eventDurationInSecs + eventStartTime > Time.time)
            {
                timerUI.text = Time.time.ToString();
            }
        }

        internal void InitEvent(int eventDurationInSecs, Text currentEventUI, Text timer)
        {
            eventStartTime = Time.time;
            this.currentEventUI = currentEventUI;
            this.timerUI = timer;
            this.eventDurationInSecs = eventDurationInSecs;
        }

        internal abstract string ToString();

        internal bool ShouldAdvanceState()
        {
            return eventDurationInSecs + eventStartTime < Time.time;
        }
    }

    public static class GameEventFactory
    {
        public static GameEvent GetGameEvent(GameState state)
        {
            switch (state)
            {
                case GameState.PlayerMovementInput:
                    return new PlayerInputEvent();
                case GameState.PlayerMovementOutput:
                    return new PlayerOutputEvent();
                case GameState.BulletMovement:
                    return new BulletMovementEvent();
                case GameState.EnemyMovement:
                    return new EnemyMovementEvent();
                case GameState.EnemiesFire:
                    return new EnemiesFireEvent();
                case GameState.PlayerAttackInput:
                    return new PlayerAttackInput();
                default:
                    return null;

            }
        }
    }
}