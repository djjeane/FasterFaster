using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAttack : MonoBehaviour
{
    public int CellAttackRange;
    public bool RangedAttack;

    public GameObject bulletPrefab;

    private PlayerMeleeAttack meleeAttack;
    private PlayerRangedAttack rangedAttack;
    // Start is called before the first frame update
    void Start()
    {

        rangedAttack = ScriptableObject.CreateInstance<PlayerRangedAttack>();
        rangedAttack.BulletPrefab = bulletPrefab;
        rangedAttack.InitRanged(transform);

        meleeAttack = ScriptableObject.CreateInstance<PlayerMeleeAttack>();
        meleeAttack.InitMelee(CellAttackRange);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(rangedAttack.attackKey))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
            var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            rangedAttack.PerformAttack(worldPoint,playerPos);
        }
        if (Input.GetKeyDown(meleeAttack.attackKey))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
            var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            meleeAttack.PerformAttack(worldPoint, playerPos);
        }
    }
 
}
