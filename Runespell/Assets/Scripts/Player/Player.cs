using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private float width = 0.1f;
    [SerializeField]
    private float height = 0.1f;
    [SerializeField] private float health = 100;
    [SerializeField] float lastHit = 0;
    [SerializeField] float damageCooldown = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Deals damage to the player
    public void TakeDamage(float amount)
    {
        if (Time.time-lastHit>damageCooldown)
        {
            lastHit = Time.time;
            health -= amount;
        }
    }

    //Get and set statements
    public float Width { get { return width; } }
    public float Height { get { return height; } }
}
