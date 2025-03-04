using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //fields
    [SerializeField]
    protected float multiplier;

    //get external manager classes
    protected ProjectileController projectileController;
    protected Player player;
    protected GameObject closestTarget;
    protected GameObject farthestTarget;


    public ProjectileController ProjectileController { get { return projectileController; } set { projectileController = value; } }
    public Player Player { get { return player; } set { player = value; } }
    public GameObject ClosestTarget { get { return closestTarget; } set { closestTarget = value; } }
    public GameObject FarthestTarget { get { return farthestTarget; } set { farthestTarget = value; } }
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //instantiate projectiles and add them to projectile list in projectileController
    public virtual void Fire()
    {

    }
    public virtual void SetProjectileMult(float mult)
    {

    }

    public virtual SpellNames GetSpellName()
    {
        return SpellNames.Fireball;
    }
}
