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
    private float width = 0.7f;
    [SerializeField]
    private float height = 0.7f;

    [SerializeField]
    private float health = 100;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Vector2 positionDifference;

    [SerializeField]
    private float damageEffectTime;

    [SerializeField]
    private GameObject corpsePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Moves enemy towards player
        Vector2 newPosition = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        positionDifference = newPosition - new Vector2(transform.position.x,transform.position.y);
        transform.position = newPosition;

        
        animations();
    }

    void animations()
    {
        if (Mathf.Abs(positionDifference.x)> Mathf.Abs(positionDifference.y))
        {
            if (positionDifference.x > 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingRight", true);
            }
            else if (positionDifference.x < 0)
            {
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingLeft", true);
            }
        }
        else
        {
            if (positionDifference.y > 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingUp", true);
            }
            else if (positionDifference.y < 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", true);
            }
        }
        if (Time.time - damageEffectTime > 0.2f)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        damageEffectTime = Time.time;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnDestroy()
    {
        GameObject corpse = Instantiate(corpsePrefab, this.gameObject.transform.position, Quaternion.identity);
        corpse.GetComponent<DyingEnemy>().SpawnTime = Time.time;
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
