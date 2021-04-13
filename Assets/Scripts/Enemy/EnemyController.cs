using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        //var thisId = this.GetInstanceID();
        //var enemyToRemove = Spawner.Enemies.FirstOrDefault(x => x.GetInstanceID() == thisId);
        //if(enemyToRemove != null)
        //{
        //    Spawner.Enemies.Remove(enemyToRemove);
        //}
        Spawner.Enemies.Remove(gameObject);
        Spawner.Enemies.RemoveAll(item => item == null);
    }
}
