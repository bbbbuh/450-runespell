using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //damage mult
    protected float multiplier;

    [SerializeField]
    protected float baseDamage;

    [SerializeField]
    protected float speed;

    protected Player player;

    protected Vector3 direction;
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    public float BaseDamage { get { return baseDamage; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
