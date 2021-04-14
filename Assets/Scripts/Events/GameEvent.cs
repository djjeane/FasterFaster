using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Events;

namespace Assets.Scripts
{
    public abstract class GameEvent : ScriptableObject
    {
        protected Text currentEventUI;
        protected Text timerUI;
        protected int eventDurationInSecs;

        internal float eventStartTime;
        internal void FireEvent()
        {
            currentEventUI.text = this.ToString(); 
            if(eventDurationInSecs + eventStartTime > Time.time)
            {
                timerUI.text = Time.time.ToString();
            }
        }

        internal virtual void InitEvent(int eventDurationInSecs, Text currentEventUI, Text timer)
        {
            eventStartTime = Time.time;
            this.currentEventUI = currentEventUI;
            this.timerUI = timer;
            this.eventDurationInSecs = eventDurationInSecs;
        }

        internal virtual void CloseEvent()
        {

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
                    return ScriptableObject.CreateInstance<PlayerInputEvent>();
                case GameState.PlayerMovementOutput:
                    return ScriptableObject.CreateInstance <PlayerOutputEvent>();
                case GameState.BulletMovement:
                    return ScriptableObject.CreateInstance <BulletMovementEvent>();
                case GameState.EnemyMovement:
                    return ScriptableObject.CreateInstance <EnemyMovementEvent>();
                case GameState.EnemiesFire:
                    return ScriptableObject.CreateInstance <EnemiesFireEvent>();
                case GameState.PlayerAttackInput:
                    return ScriptableObject.CreateInstance <PlayerAttackInput>();
                default:
                    return null;

            }
        }
    }
}