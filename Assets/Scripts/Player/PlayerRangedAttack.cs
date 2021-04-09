using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : Attack
{
    public override KeyCode attackKey { get ; set; }
    public override int CellAttackRange { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override Transform attackSourceLocation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public PlayerRangedAttack()
    {
        attackKey = KeyCode.Space;
    }

    internal override void PerformAttack(Vector3Int locationToAttack)
    {
        var playerPosition = new Vector3Int(Mathf.FloorToInt(attackSourceLocation.position.x), Mathf.FloorToInt(attackSourceLocation.position.y), 0);

    }
}
