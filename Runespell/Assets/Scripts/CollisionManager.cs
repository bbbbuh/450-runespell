using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] ProjectileController projectileController;
    [SerializeField] GameObject player;
    [SerializeField] float playerHeight = 0.1f;
    [SerializeField] float playerWidth = 0.1f;
    [SerializeField] GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerEnemyCollision();
        ProjectileEnemyCollision();
        PlayerExitCollision();
    }

    //Handles collisions between enemies and players
    //Also damages player
    void PlayerEnemyCollision()
    {
        List<GameObject> enemyList = enemyManager.EnemyList;
        Vector2 playerPos = player.transform.position;
        float playerWidth = player.GetComponent<Player>().Width;
        float playerHeight = player.GetComponent<Player>().Height;
        if (enemyList != null)
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
                    if (CheckCollision(projectiles[j].transform.position, 0.2f, 0.2f, enemyList[i].transform.position,
                        enemyList[i].GetComponent<Enemy>().Width, enemyList[i].GetComponent<Enemy>().Height))
                    {
                        switch (projectiles[j].SpellName)
                        {
                            case SpellNames.Fireball:
                                // UnityEngine.Debug.Log("MULT: " + projectiles[j].Multiplier);
                                enemyList[i].GetComponent<Enemy>().TakeDamage(projectiles[j].BaseDamage); // * projectiles[j].Multiplier;
                                SoundManager.instance.PlaySoundEffect(SoundEffectNames.EnemyHurt);
                                projectiles[j].Used = true;
                                break;
                            case SpellNames.MagicOrb:
                                SoundManager.instance.PlaySoundEffect(SoundEffectNames.MagicOrbExplosion);
                                List<GameObject> impactedEnemyList = new List<GameObject>();
                                float explosionRadius = 1.0f; // * projectiles[j].Multiplier

                                for (int k = enemyList.Count - 1; k >= 0; k--)
                                {
                                    if (Vector3.Distance(enemyList[k].transform.position, enemyList[i].transform.position) <= explosionRadius)
                                    {
                                        impactedEnemyList.Add(enemyList[k]);
                                    }
                                }

                                for (int m = impactedEnemyList.Count - 1; m >= 0; m--)
                                {
                                    impactedEnemyList[m].GetComponent<Enemy>().TakeDamage(projectiles[j].BaseDamage);
                                    SoundManager.instance.PlaySoundEffect(SoundEffectNames.EnemyHurt);
                                }

                                // SoundManager.instance.PlaySoundEffect(SoundEffectNames.MagicOrbExplosion);
                                projectiles[j].Used = true;
                                break;
                            default: break;
                        }



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

    void PlayerExitCollision()
    {
        if (enemyManager.Exit != null)
        {
            if (CheckCollision(enemyManager.Exit.transform.position, 1, 1, player.transform.position, player.GetComponent<Player>().Width, player.GetComponent<Player>().Height))
            {
                if (enemyManager.Exit.GetComponent<Door>().Open)
                {
                    gameManager.GetComponent<GameManager>().NextScene = true;
                    Debug.Log("Next Scene");
                }
            }
        }
    }

    //Checks collision between any two objects
    bool CheckCollision(Vector2 pos1, float width1, float height1, Vector2 pos2, float width2, float height2)
    {
        pos1.x -= width1 / 2;
        pos1.y -= height1 / 2;
        pos2.x -= width2 / 2;
        pos2.y -= height2 / 2;
        return (pos1.x < pos2.x + width2 && pos1.x + width1 > pos2.x && pos1.y < pos2.y + height2 && pos1.y + height1 > pos2.y);
    }
}
