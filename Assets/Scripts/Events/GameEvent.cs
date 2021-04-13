using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;


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
        private HashSet<Vector3Int> destinationTilesHashset;
        internal override void InitEvent(int eventDurationInSecs, Text currentEventUI, Text timer)
        {
            eventStartTime = Time.time;
            this.currentEventUI = currentEventUI;
            this.timerUI = timer;
            this.eventDurationInSecs = eventDurationInSecs;

            destinationTilesHashset = new HashSet<Vector3Int>();

            var enemies = Spawner.Enemies;


            //This logic will break if the enemy has no available square to move to, do better
            foreach(var enemy in enemies)
            {
                var deniedDestinations = new List<Vector3Int>();
                var foundGoodDestination = false;
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                while (!foundGoodDestination)
                {
                    var destinationSq = movement.GetDestinationMoveTileOfEnemy(deniedDestinations);

                    if(destinationSq != Vector3Int.zero)
                    {
                        if (!destinationTilesHashset.Contains(destinationSq))
                        {
                            movement.SetDestination(destinationSq);
                            destinationTilesHashset.Add(destinationSq);
                            foundGoodDestination = true;
                        }
                        else
                        {
                            //If we already have an enemy headed there, lets find a new place to go
                            // add the square to the list of tiles to avoid
                            deniedDestinations.Add(destinationSq);

                        }
                    }
                    else
                    {
                        //destination could not be gotten
                        //just move on 
                        foundGoodDestination = true;
                    }
                  
                }
             

            }
            

        }
        internal override void CloseEvent()
        {
            destinationTilesHashset = null;
        }

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