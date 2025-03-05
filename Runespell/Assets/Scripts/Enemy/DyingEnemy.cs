using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingEnemy : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime >= 0.5f) 
        {
            Destroy(this.gameObject);
        }
    }

    public float SpawnTime { get { return spawnTime; } set { spawnTime = value; } }

}
