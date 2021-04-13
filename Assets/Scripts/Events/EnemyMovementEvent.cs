using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Events
{
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
            var deniedDestinations = new List<Vector3Int>();
            foreach (var enemy in enemies)
            {
                var foundGoodDestination = false;
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                while (!foundGoodDestination)
                {
                    var destinationSq = movement.GetDestinationMoveTileOfEnemy(deniedDestinations);

                    if (destinationSq != Vector3Int.zero)
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
}