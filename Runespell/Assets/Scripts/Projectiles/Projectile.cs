using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellNames
{
    Fireball,
    Heal,
    MagicOrb
}

public class Projectile : MonoBehaviour
{
    //damage mult
    protected float multiplier;

    [SerializeField]
    protected float baseDamage;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected bool used;

    [SerializeField]
    protected float timeCast;

    protected Player player;

    protected SpellNames spellName;

    protected Vector3 direction;
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    public float BaseDamage { get { return baseDamage; } }

    public SpellNames SpellName { get { return spellName; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Used
    {
        get { return used; }
        set { used = value; }
    }
    public float TimeCast 
    {  get { return timeCast; } set {  timeCast = value; } }

}
