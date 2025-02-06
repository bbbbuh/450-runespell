using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireballProjectile : Projectile
{
    

    // Start is called before the first frame update
    void Start()
    {
        //get direction of the spell from its rotation
        baseDamage = 25.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //move projectile to target position
        Vector2 newPosition = transform.position + (direction * speed * Time.deltaTime);
        transform.position = newPosition;
    }

    
}
