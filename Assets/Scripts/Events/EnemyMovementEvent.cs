using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Events
{
    internal class EnemyMovementEvent : GameEvent
    {
        private HashSet<Vector3Int> claimedDestinationsHasSet;
        internal override void InitEvent(int eventDurationInSecs, Text currentEventUI, Text timer)
        {
            eventStartTime = Time.time;
            this.currentEventUI = currentEventUI;
            this.timerUI = timer;
            this.eventDurationInSecs = eventDurationInSecs;

            claimedDestinationsHasSet = new HashSet<Vector3Int>();

            var enemies = Spawner.Enemies;


            //This logic will break if the enemy has no available square to move to, do better
            var deniedDestinations = new List<Vector3Int>();
            foreach (var enemy in enemies)
            {
                var foundGoodDestination = false;
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                while (!foundGoodDestination)
                {
                    var destinationSq = movement.GetDestinationMoveTileOfEnemy(claimedDestinationsHasSet.ToList());

                    if (destinationSq != Vector3Int.zero)
                    {
                        if (!claimedDestinationsHasSet.Contains(destinationSq))
                        {
                            movement.SetDestination(destinationSq);
                            claimedDestinationsHasSet.Add(destinationSq);
                            foundGoodDestination = true;
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
            claimedDestinationsHasSet = null;
        }

        internal override string ToString()
        {
            return "Enemies Move";
        }
    }
}