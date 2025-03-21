using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSlotManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> slots = new List<GameObject>(3);

    [SerializeField]
    private List<bool> hasSpellInside = new List<bool>(3);

    [SerializeField]
    private ProjectileController projectileController;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject twoSecBar;

    [SerializeField]
    private GameObject fiveSecBar;

    [SerializeField]
    private GameObject tenSecBar;

    private RectTransform twoSecBarTransform;
    private RectTransform fiveSecBarTransform;
    private RectTransform tenSecBarTransform;
    
    [SerializeField]
    private float maxHeight = 100.0f;

    [SerializeField]
    private float maxWidth = 88.0f;

    public List<GameObject> Slots { get { return slots; } set { slots = value; } }
    public ProjectileController ProjectileController { get { return projectileController; } set { projectileController = value; } }

    //[SerializeField]
    //private List<SpellNames> spellNameList = new List<SpellNames>(3);

    //public List<SpellNames> SpellNameList { get { return spellNameList; } }

    // Start is called before the first frame update
    void Start()
    {
        projectileController = GameObject.Find("ProjectileController").GetComponent<ProjectileController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        twoSecBarTransform = twoSecBar.GetComponent<RectTransform>();
        fiveSecBarTransform = fiveSecBar.GetComponent<RectTransform>();
        tenSecBarTransform = tenSecBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame 
    void Update()
    {
        UpdateCastingBars();
    }

    public void AddSpellToProjectileManager(Spell spell, int slot)
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (slot == 0)
        {
            projectileController.TwoSecSlot = spell;
            projectileController.InstantiateManagers(spell, 1.0f);
            gameManager.savedSpellNames[0] = spell.GetSpellName();
            hasSpellInside[0] = true;
        }
        else if (slot == 1)
        {
            projectileController.FiveSecSlot = spell;
            projectileController.InstantiateManagers(spell, 3.0f);
            gameManager.savedSpellNames[1] = spell.GetSpellName();
            hasSpellInside[1] = true;
        }
        else if (slot == 2)
        {
            projectileController.TenSecSlot = spell;
            projectileController.InstantiateManagers(spell, 8.0f);
            gameManager.savedSpellNames[2] = spell.GetSpellName();
            hasSpellInside[2] = true;
        }

        //UnityEngine.Debug.Log("SPELL NAME LIST 1: " + gameManager.savedSpellNames[0]);
        //UnityEngine.Debug.Log("SPELL NAME LIST 2: " + gameManager.savedSpellNames[1]);
        //UnityEngine.Debug.Log("SPELL NAME LIST 3: " + gameManager.savedSpellNames[2]);
    }

    // NOTE: Setting max health to 100. If this causes issues in the future, it's probably that

    // In case you're wondering why this is so big, blame the Heal Spell
    // Since the Heal Spell only activates when you have less than max health
    // and is also the only spell that can function outside of combat
    // we need all this extra code to disable Healing during combat when you're at max health
    // and enable Healing when you're out of combat but not at full health
    // Btw, when I say "enable Healing" I specifically mean enable the bar for the heal spell
    // if there is a heal spell that the player is currently holding. Actual healing is done elsewhere

    public void UpdateCastingBars()
    {
        bool[] isHeal = { false, false, false };

        for (int i = 0; i < 3; i++)
        {
            //UnityEngine.Debug.Log("ONE: " + (hasSpellInside[i] == true));
            //UnityEngine.Debug.Log("TWO: " + (gameManager.savedSpellNames[i] == SpellNames.Heal));
            //UnityEngine.Debug.Log("THREE: " + (gameManager.PlayerHealth < 100));

            if (hasSpellInside[i]
                && gameManager.savedSpellNames[i] == SpellNames.Heal
                && gameManager.PlayerHealth < 100)
            {
                isHeal[i] = true;
            }
        }

        //UnityEngine.Debug.Log("ONE: " + isHeal[0]);
        //UnityEngine.Debug.Log("TWO: " + isHeal[1]);
        //UnityEngine.Debug.Log("THREE: " + isHeal[2]);
        //UnityEngine.Debug.Log("WE BATTLING?? " + (SoundManager.instance.CurrentGameState == GameState.Calm));

        if (SoundManager.instance.CurrentGameState == GameState.Calm)
        {
            if (isHeal[0] == false)
            {
                twoSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.TwoSecTimer, 0.0f, 2.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 2.0f);
                twoSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }

            if (isHeal[1] == false)
            {
                fiveSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.FiveSecTimer, 0.0f, 5.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 5.0f);
                fiveSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }

            if (isHeal[2] == false)
            {
                tenSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.TenSecTimer, 0.0f, 10.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 10.0f);
                tenSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }

            return;
        }

        // DURING BATTLE

        if (hasSpellInside[0])
        {
            if (gameManager.savedSpellNames[0] == SpellNames.Heal &&
                gameManager.playerHealth >= 100)
            {
                twoSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.TwoSecTimer, 0.0f, 2.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 2.0f);
                twoSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }
        }
        if (hasSpellInside[1])
        {
            if (gameManager.savedSpellNames[1] == SpellNames.Heal &&
                gameManager.playerHealth >= 100)
            {
                fiveSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.FiveSecTimer, 0.0f, 5.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 5.0f);
                fiveSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }
        }
        if (hasSpellInside[2])
        {
            if (gameManager.savedSpellNames[2] == SpellNames.Heal &&
                gameManager.playerHealth >= 100)
            {
                tenSecBarTransform.sizeDelta = new Vector2(0.0f, 0.0f);
            }
            else
            {
                float timer = Mathf.Clamp(ProjectileController.TenSecTimer, 0.0f, 10.0f);
                float newHeight = Mathf.Lerp(0.0f, maxHeight, timer / 10.0f);
                tenSecBarTransform.sizeDelta = new Vector2(maxWidth, newHeight);
            }
        }
    }
}
