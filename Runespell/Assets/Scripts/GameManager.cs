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
    [SerializeField] public float playerHealth;
    [SerializeField] SpellSlotManager spellSlotManager;
    [SerializeField] bool spell1;
    [SerializeField] bool spell2;
    [SerializeField] bool spell3;
    [SerializeField] GameObject projectileManager;
    [SerializeField] FireballSpell fireball;
    [SerializeField] MagicOrbSpell magicOrb;
    [SerializeField] HealSpell heal;
    [SerializeField] GameObject collisionManager;
    [SerializeField] GameObject enemyManager;

    [SerializeField]
    public List<SpellNames> savedSpellNames = new List<SpellNames>(3);

    public float PlayerHealth { get { return playerHealth; } }

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
        playerHealth = player.GetComponent<Player>().Health;

        PlayerDeath();
        SceneTransition();
    }

    void PlayerDeath()
    {
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
            nextScene = false;
            playerHealth = 100;
            Destroy(this.gameObject);
        }
    }

    void SceneTransition()
    {
        if (nextScene)
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectNames.Door);
            
            //playerHealth = player.GetComponent<Player>().Health;

            //savedSpellNames = projectileManager.GetComponent<ProjectileController>().GetSpellNameList();

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

            currentScene = SceneManager.GetActiveScene().name;
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
            nextScene = false;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nextScene = false;
        currentScene = SceneManager.GetActiveScene().name;
        player = GameObject.Find("Player");
        projectileManager = GameObject.Find("ProjectileController");
        spellSlotManager = GameObject.Find("SpellSlotManager").GetComponent<SpellSlotManager>();
        collisionManager = GameObject.Find("CollisionManager");
        enemyManager = GameObject.Find("EnemyManager");

        projectileManager.GetComponent<ProjectileController>().Player = player.GetComponent<Player>();
        projectileManager.GetComponent<ProjectileController>().CollisionManager = collisionManager.GetComponent<CollisionManager>();
        projectileManager.GetComponent<ProjectileController>().EnemyManager = enemyManager.GetComponent<EnemyManager>();

        player.GetComponent<Player>().Health = playerHealth;
        
        spellSlotManager.GetComponent<SpellSlotManager>().ProjectileController = projectileManager.GetComponent<ProjectileController>();
        if (spell1)
        {
            LoadSpells(savedSpellNames[0], 0);
            //spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, 0);
            //spellSlotManager.Slots[0].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = fireball.GetComponent<SpriteRenderer>().sprite;
        }
        if (spell2)
        {
            LoadSpells(savedSpellNames[1], 1);
            //spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, 1);
            //spellSlotManager.Slots[1].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = fireball.GetComponent<SpriteRenderer>().sprite;
        }
        if (spell3)
        {
            LoadSpells(savedSpellNames[2], 2);
            //spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(heal, 2);
            //spellSlotManager.Slots[2].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = heal.GetComponent<SpriteRenderer>().sprite;
        }
    }

    void LoadSpells(SpellNames name, int slotNum)
    {
        switch (name)
        {
            case SpellNames.Fireball:
                spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(fireball, slotNum);
                spellSlotManager.Slots[slotNum].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = fireball.GetComponent<SpriteRenderer>().sprite;
                break;
            case SpellNames.Heal:
                spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(heal, slotNum);
                spellSlotManager.Slots[slotNum].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = heal.GetComponent<SpriteRenderer>().sprite;
                break;
            case SpellNames.MagicOrb:
                spellSlotManager.GetComponent<SpellSlotManager>().AddSpellToProjectileManager(magicOrb, slotNum);
                spellSlotManager.Slots[slotNum].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = magicOrb.GetComponent<SpriteRenderer>().sprite;
                break;
            default: break;
        }
    }

    public bool NextScene { get { return nextScene; } set { nextScene = value; } }
}
