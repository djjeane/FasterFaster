using Assets.Scripts;
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

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rangedAttack = ScriptableObject.CreateInstance<PlayerRangedAttack>();
        rangedAttack.BulletPrefab = bulletPrefab;
        rangedAttack.InitRanged(transform);

        meleeAttack = ScriptableObject.CreateInstance<PlayerMeleeAttack>();
        meleeAttack.InitMelee(CellAttackRange);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EventsManager.currentState == GameState.PlayerAttackInput)
        {
            HandleAttacks();
        }
    }

    private void HandleAttacks()
    {
        Vector3 point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
        var playerPos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
        HandleRangedAttack(playerPos,worldPoint);
        HandleMeleeAttack(playerPos, worldPoint);
    }


    private void HandleMeleeAttack(Vector3Int playerPos, Vector3Int worldPoint)
    {
        if (Input.GetKeyDown(meleeAttack.attackKey))
        {
            meleeAttack.PerformAttack(worldPoint, playerPos);
        }
    }

    private void HandleRangedAttack(Vector3Int playerPos, Vector3Int worldPoint)
    {
        if (Input.GetKeyDown(rangedAttack.attackKey))
        {
            rangedAttack.PerformAttack(worldPoint, playerPos);
        }
    }
 
}
