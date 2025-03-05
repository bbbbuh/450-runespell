using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireballProjectile : Projectile
{

    [SerializeField] GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //get direction of the spell from its rotation
        baseDamage = 25.0f;
        spellName = SpellNames.Fireball;
    }

    // Update is called once per frame
    void Update()
    {
        //move projectile to target position
        Vector2 newPosition = transform.position + (direction * speed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void OnDestroy()
    {
        GameObject explosion = Instantiate(explosionPrefab, this.gameObject.transform.position, Quaternion.identity);
        explosion.GetComponent<FireballExplosion>().SpawnTime = Time.time;
    }


}
