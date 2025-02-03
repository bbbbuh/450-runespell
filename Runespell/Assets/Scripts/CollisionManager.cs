using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject player;
    [SerializeField] float playerHeight = 0.1f;
    [SerializeField] float playerWidth = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerEnemyCollision();
    }

    //Handles collisions between enemies and players
    //Also damages player
    void PlayerEnemyCollision()
    {
        List<GameObject> enemyList = enemyManager.EnemyList;
        Vector2 playerPos = player.transform.position;
        float playerWidth = player.GetComponent<Player>().Width;
        float playerHeight = player.GetComponent<Player>().Height;
        if (enemyList!=null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (CheckCollision(playerPos, playerWidth, playerHeight, enemyList[i].transform.position, 
                    enemyList[i].GetComponent<Enemy>().Width, enemyList[i].GetComponent<Enemy>().Height))
                {
                    player.GetComponent<Player>().TakeDamage(enemyList[i].GetComponent<Enemy>().Damage);
                }
            }
        }
        
    }

    //Checks collision between any two objects
    bool CheckCollision(Vector2 pos1, float width1, float height1, Vector2 pos2, float width2, float height2)
    {
        return (pos1.x < pos2.x + width2 && pos1.x + width1 > pos2.x && pos1.y < pos2.y + height2 && pos1.y + height1 > pos2.y);
    }
}
