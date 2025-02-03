using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnEnemy", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCleanup();
    }

    //Spawns an enemy
    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, new Vector2(1f, 1f), Quaternion.identity);
        enemy.GetComponent<Enemy>().Target= player;
        enemyList.Add(enemy);
    }

    //Removes dead enemies
    void EnemyCleanup()
    {
        if (enemyList != null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != null)
                {
                    if (enemyList[i].GetComponent<Enemy>().Health < 0)
                    {
                        Destroy(enemyList[i]);
                        enemyList.RemoveAt(i);
                    }
                }
            }
        }
        
    }

    //Get and set statements
    public List<GameObject> EnemyList { 
        get { return enemyList; } 
    }
}
