using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public static class GameEventFactory
    {
        public static GameEvent GetGameEvent(GameState state)
        {
            switch (state)
            {
                case GameState.PlayerMovementInput:
                    return ScriptableObject.CreateInstance<PlayerInputEvent>();
                case GameState.PlayerMovementOutput:
                    return ScriptableObject.CreateInstance<PlayerOutputEvent>();
                case GameState.BulletMovement:
                    return ScriptableObject.CreateInstance<BulletMovementEvent>();
                case GameState.EnemyMovement:
                    return ScriptableObject.CreateInstance<EnemyMovementEvent>();
                case GameState.EnemiesFire:
                    return ScriptableObject.CreateInstance<EnemiesFireEvent>();
                case GameState.PlayerAttackInput:
                    return ScriptableObject.CreateInstance<PlayerAttackInput>();
                default:
                    return null;

            }
        }
    }
}