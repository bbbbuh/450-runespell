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
    [SerializeField] float health = 100;


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
        health -= amount;
    }

    //Get and set statements
    public float Width { get { return width; } }
    public float Height { get { return height; } }
}
