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
        InvokeRepeating("SpawnEnemy", 2.0f, 2.0f);
        //SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        EnemyCleanup();
    }

    //Spawns an enemy
    void SpawnEnemy()
    {
        int spawnLocation = Random.Range(1, 5);
        Vector2 coordinates;
        if (spawnLocation == 1)
        {
            coordinates = new Vector2(-10, 5);
        }
        else if (spawnLocation == 2) 
        {
            coordinates = new Vector2(10, 5);
        }
        else if (spawnLocation == 3)
        {
            coordinates = new Vector2(-10, -5);
        }
        else
        {
            coordinates = new Vector2(10, -5);
        }

        GameObject enemy = Instantiate(enemyPrefab, coordinates, Quaternion.identity);
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
