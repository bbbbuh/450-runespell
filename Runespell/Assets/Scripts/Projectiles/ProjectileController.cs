using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //managers
    protected CollisionManager collisionManager;
    protected EnemyManager enemyManager;
    protected Player player;

    //list of projectiles
    [SerializeField]
    private List<Projectile> projectiles;

    public List<Projectile> Projectiles { get { return projectiles; } set { projectiles = value; } }

    //spell slots
    [SerializeField]
    private Spell twoSecSlot;
    [SerializeField]
    private Spell fiveSecSlot;
    [SerializeField]
    private Spell tenSecSlot;


    //timers
    private float twoSecTimer = 0;
    private float fiveSecTimer = 0;
    private float tenSecTimer = 0;

    //targets
    private GameObject closestTarget;
    private GameObject farthestTarget;

    public GameObject ClosestTarget { get { return closestTarget; } set { closestTarget = value; } }
    public GameObject FarthestTarget { get { return farthestTarget; } set { farthestTarget = value; } }
    public Spell TwoSecSlot {  get { return twoSecSlot; } set {  twoSecSlot = value; } }
    public Spell FiveSecSlot { get { return fiveSecSlot; } set { fiveSecSlot = value; } }
    public Spell TenSecSlot { get { return tenSecSlot; } set { tenSecSlot = value; } }
    public CollisionManager CollisionManager { get { return collisionManager; } set { collisionManager = value; } }
    public EnemyManager EnemyManager { get { return enemyManager; } set { enemyManager = value; } }
    public Player Player { get { return player; } set {  player = value; } }

    public float TwoSecTimer { get { return twoSecTimer; } }
    public float FiveSecTimer { get { return fiveSecTimer; } }
    public float TenSecTimer { get { return tenSecTimer; } }

    // Start is called before the first frame update
    void Start()
    {
        //set collisionManager from in the scene
        collisionManager = GameObject.Find("CollisionManager").GetComponent<CollisionManager>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        //find closest and farthest enemies
        float closest = 10000.0f;
        float farthest = 0.0f;
        foreach (GameObject e in enemyManager.EnemyList)
        {
            float distance = Vector2.Distance(player.transform.position, e.GetComponent<Enemy>().transform.position);
            if (distance < closest)
            {
                closest = distance;
                closestTarget = e;
            }

            if (distance > farthest)
            {
                farthest = distance;
                farthestTarget = e;
            }
        }

        //incremement timers
        twoSecTimer += Time.deltaTime;
        fiveSecTimer += Time.deltaTime;
        tenSecTimer += Time.deltaTime;

        //fire spells when timer finishes
        if (twoSecTimer >= 2.0f && twoSecSlot != null)
        {
            twoSecTimer = 0;
            twoSecSlot.Fire();
        }
        if (fiveSecTimer >= 5.0f && fiveSecSlot != null)
        {
            fiveSecTimer = 0;
            fiveSecSlot.Fire();
        }
        if (tenSecTimer >= 10.0f && tenSecSlot != null)
        {   
            tenSecTimer = 0;
            tenSecSlot.Fire();
        }
    }

    private void LateUpdate()
    {
        projectileCleanup();
    }

    public void InstantiateManagers(Spell spell, float mult)
    {
        if (spell != null)
        {
            spell.ProjectileController = this;
            spell.Player = player;
            spell.Multiplier = mult;
            spell.SetProjectileMult(mult);
        }

    }

    //Removes used projectiles
    void projectileCleanup()
    {
        if (projectiles != null)
        {
            for (int i = projectiles.Count-1; i >= 0; i--)
            {
                if (projectiles[i] != null)
                {
                    if (projectiles[i].GetComponent<Projectile>().Used || Time.time - projectiles[i].GetComponent<Projectile>().TimeCast >5)
                    {
                        Destroy(projectiles[i].gameObject);
                        projectiles.RemoveAt(i);
                    }
                }
            }
        }
    }

    //public List<SpellNames> GetSpellNameList()
    //{
    //    List<SpellNames> temp = new List<SpellNames>();
    //    temp.Add(TwoSecSlot.GetSpellName());
    //    temp.Add(FiveSecSlot.GetSpellName());
    //    temp.Add(TenSecSlot.GetSpellName());
    //    return temp;
    //}
}
