using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] ProjectileController projectileController;
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
        ProjectileEnemyCollision();
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

    //handles collisions between projectiles and enemies
    //damages enemies and handles death for enemies and projectiles
    void ProjectileEnemyCollision()
    {
        List<GameObject> enemyList = enemyManager.EnemyList;
        List<Projectile> projectiles = projectileController.Projectiles;
        if (enemyList != null && projectiles != null) 
        {
            for (int j = projectiles.Count - 1; j >= 0; j--)
            {
                for (int i = enemyList.Count - 1; i >= 0; i--)
                {
                    Debug.Log(j);
                    if (CheckCollision(projectiles[j].transform.position, 1.0f, 1.0f, enemyList[i].transform.position,
                        enemyList[i].GetComponent<Enemy>().Width, enemyList[i].GetComponent<Enemy>().Height))
                    {
                        enemyList[i].GetComponent<Enemy>().Health -= projectiles[j].BaseDamage;
                        projectiles[j].Used=true;

                        /*if (enemyList[i].Health <= 0)
                        {
                            Enemy tempEnemy = enemyList[i];
                            enemyList.RemoveAt(i);
                            Destroy(tempEnemy.gameObject);
                        }*/
                    }
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
