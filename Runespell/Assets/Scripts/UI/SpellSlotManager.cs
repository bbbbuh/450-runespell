using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSlotManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> slots = new List<GameObject>(3);

    [SerializeField]
    private ProjectileController projectileController;

    [SerializeField]
    private GameManager gameManager;

    public List<GameObject> Slots {  get { return slots; } set { slots = value; } }
    public ProjectileController ProjectileController { get {  return projectileController; } set {  projectileController = value; } }

    //[SerializeField]
    //private List<SpellNames> spellNameList = new List<SpellNames>(3);

    //public List<SpellNames> SpellNameList { get { return spellNameList; } }

    // Start is called before the first frame update
    void Start()
    {
        projectileController = GameObject.Find("ProjectileController").GetComponent<ProjectileController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame 
    void Update()
    {
        
    }

    public void AddSpellToProjectileManager(Spell spell, int slot)
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (slot == 0)
        {
            projectileController.TwoSecSlot = spell;
            projectileController.InstantiateManagers(spell, 1.0f);
            gameManager.savedSpellNames[0] = spell.GetSpellName();
        }
        else if (slot == 1)
        {
            projectileController.FiveSecSlot = spell;
            projectileController.InstantiateManagers(spell, 3.0f);
            gameManager.savedSpellNames[1] = spell.GetSpellName();
        }
        else if (slot == 2)
        {
            projectileController.TenSecSlot = spell;
            projectileController.InstantiateManagers(spell, 8.0f);
            gameManager.savedSpellNames[2] = spell.GetSpellName();
        }

        //UnityEngine.Debug.Log("SPELL NAME LIST 1: " + gameManager.savedSpellNames[0]);
        //UnityEngine.Debug.Log("SPELL NAME LIST 2: " + gameManager.savedSpellNames[1]);
        //UnityEngine.Debug.Log("SPELL NAME LIST 3: " + gameManager.savedSpellNames[2]);
    }
}
