using System;
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

    // Spells

    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject heal;
    [SerializeField] private GameObject magicOrb;

    // Used to switch songs only once
    private bool noEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnEnemy", 2.0f, 2.0f);
        SpawnEnemy();
        SpawnExit();
        SoundManager.instance.SwitchSong(GameState.Battle);
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
        foreach (Transform child in spawnLocations)
        {
            if (child.gameObject.tag == tag)
            {
                if (enemyList.Count < numEnemies)
                {
                    GameObject enemy = Instantiate(enemyPrefab, child.gameObject.transform.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().Target = player;
                    enemyList.Add(enemy);
                    noEnemies = false;
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
                        SpawnSpellDrop(enemyList[i].transform.position);
                        Destroy(enemyList[i]);
                        enemyList.RemoveAt(i);
                    }
                }
            }
        }
        if (enemyList.Count == 0)
        {
            if (noEnemies == false)
            {
                SoundManager.instance.SwitchSong(GameState.Calm);
                exit.GetComponent<Door>().OpenDoor();
                noEnemies = true;
            }
        }
    }

    void SpawnExit()
    {
        exit = Instantiate(exitPrefab, new Vector2(0, 4.5f), Quaternion.identity);
    }

    void SpawnSpellDrop(Vector2 position)
    {
        float spellCheck = UnityEngine.Random.Range(0.0f, 100.0f);
        //UnityEngine.Debug.Log("Spell Check: " + spellCheck);
        if (spellCheck <= 25.0f)
        {
            int spellSelect = (int)UnityEngine.Random.Range(0.0f, 3.0f);
            switch (spellSelect)
            {
                case 0: Instantiate(fireball, position, Quaternion.identity); break;
                case 1: Instantiate(heal, position, Quaternion.identity); break;
                case 2: Instantiate(magicOrb, position, Quaternion.identity); break;
                default: Instantiate(magicOrb, position, Quaternion.identity); break;
            }
        }
    }

    //Get and set statements
    public List<GameObject> EnemyList { 
        get { return enemyList; } 
    }
    public GameObject Exit {  get { return exit; } }
}
