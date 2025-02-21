using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string currentScene;
    [SerializeField] bool nextScene;
    [SerializeField] GameObject player;
    [SerializeField] List<string> sceneNames;
    [SerializeField] float playerHealth;
    [SerializeField] GameObject spellSlotManager;
    [SerializeField] bool spell1;
    [SerializeField] bool spell2;
    [SerializeField] bool spell3;
    [SerializeField] GameObject projectileManager;
    [SerializeField] Spell fireball;
    [SerializeField] GameObject collisionManager;
    [SerializeField] GameObject enemyManager;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    

    // Update is called once per frame
    void Update()
    {
        PlayerDeath();
        SceneTransition();
        
    }

    void PlayerDeath()
    {
        if (player.GetComponent<Player>().Health<=0)
        {
            SceneManager.LoadScene("LoseScreen");
            nextScene = false;
            Destroy(this.gameObject);
        }
    }

    void SceneTransition()
    {
        if (nextScene)
        {
            nextScene = false;
            playerHealth = player.GetComponent<Player>().Health;

            if (projectileManager.GetComponent<ProjectileController>().TwoSecSlot != null)
            {
                spell1 = true;
            }

            if (projectileManager.GetComponent<ProjectileController>().FiveSecSlot != null)
            {
                spell2 = true;
            }

            if (projectileManager.GetComponent<ProjectileController>().TenSecSlot != null)
            {
                spell3 = true;
            }
            
            Debug.Log(spell1);
            if (currentScene == "Room_Tutorial1")
            {
                currentScene = "Room_Tutorial2";
                SceneManager.LoadScene("Room_Tutorial2");
            }
            else if (currentScene == "Room_Tutorial2")
            {
                currentScene = "Room_Tutorial3";
                SceneManager.LoadScene("Room_Tutorial3");
            }
            else
            {
                currentScene = sceneNames[Random.Range(0, sceneNames.Count)];
                SceneManager.LoadScene(currentScene);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene().name;
        player = GameObject.Find("Player");
        projectileManager = GameObject.Find("ProjectileController");
        spellSlotManager = GameObject.Find("SpellSlotManager");
        collisionManager = GameObject.Find("CollisionManager");
        enemyManager = GameObject.Find("EnemyManager");

        projectileManager.GetComponent<ProjectileController>().Player = player.GetComponent<Player>();
        projectileManager.GetComponent<ProjectileController>().CollisionManager = collisionManager.GetComponent<CollisionManager>();
        projectileManager.GetComponent<ProjectileController>().EnemyManager = enemyManager.GetComponent<EnemyManager>();

        player.GetComponent<Player>().Health = playerHealth;
        
        spellSlotManager.GetComponent<SpellSlotManager>().ProjectileController = projectileManager.GetComponent<ProjectileController>();
        if (spell1)
        {
            spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, 0);
        }
        if (spell2)
        {
            spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, 1);
        }
        if (spell3)
        {
            spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, 2);
        }
        nextScene = false;
    }

    public bool NextScene { get { return nextScene; } set { nextScene = value; } }
}
