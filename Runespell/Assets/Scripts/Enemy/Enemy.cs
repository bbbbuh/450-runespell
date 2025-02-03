using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float damage = 10;

    [SerializeField]
    private float width = 1;
    [SerializeField]
    private float height = 1;

    [SerializeField]
    private float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Moves enemy towards player
        Vector2 newPosition = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = newPosition;
    }


    //Get and set statements
    public GameObject Target {  
        get { return target; } 
        set { target = value; }   
    }
    public float Damage { 
        get { return damage; } 
        set {  damage = value; } 
    }
    public float Width { get { return width; } }
    public float Height { get { return height; } }
    public float Health { get { return health; } set { health = value; } }
}
