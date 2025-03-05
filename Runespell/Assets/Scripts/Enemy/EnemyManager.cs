using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnLocations;
    [SerializeField] private float numEnemies;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private GameObject exit;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnEnemy", 2.0f, 2.0f);
        SpawnEnemy();
        SpawnExit();
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
        SoundManager.instance.SwitchSong(GameState.Battle);

        foreach (Transform child in spawnLocations)
        {
            if (child.gameObject.tag == tag)
            {
                if (enemyList.Count < numEnemies)
                {
                    GameObject enemy = Instantiate(enemyPrefab, child.gameObject.transform.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().Target = player;
                    enemyList.Add(enemy);
                }
            }
        }
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
                        SoundManager.instance.PlaySoundEffect(SoundEffectNames.EnemyDeath);
                        Destroy(enemyList[i]);
                        enemyList.RemoveAt(i);
                    }
                }
            }
        }
        if (enemyList.Count == 0)
        {
            SoundManager.instance.SwitchSong(GameState.Calm);
            exit.GetComponent<Door>().OpenDoor();
        }
    }

    void SpawnExit()
    {
        exit = Instantiate(exitPrefab, new Vector2(0, 4.5f), Quaternion.identity);
    }

    //Get and set statements
    public List<GameObject> EnemyList { 
        get { return enemyList; } 
    }
    public GameObject Exit {  get { return exit; } }
}
