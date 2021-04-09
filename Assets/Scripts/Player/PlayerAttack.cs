using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    public int CellAttackRange;
    public bool RangedAttack;

    public GameObject bulletPrefab;

    private Attack attack;
    // Start is called before the first frame update
    void Start()
    {
        if (RangedAttack)
        {
            attack = ScriptableObject.CreateInstance<PlayerRangedAttack>();
           
        }
        else
        {
            attack = ScriptableObject.CreateInstance<PlayerMeleeAttack>();
            attack.InitMelee(CellAttackRange, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(attack.attackKey))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
            attack.PerformAttack(worldPoint);
          
        }
    }
 
}
