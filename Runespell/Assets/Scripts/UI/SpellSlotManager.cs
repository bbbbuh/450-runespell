using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSlotManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> slots = new List<GameObject>(3);

    [SerializeField]
    private ProjectileController projectileController;

    public List<GameObject> Slots {  get { return slots; } set { slots = value; } }
    public ProjectileController ProjectileController { get {  return projectileController; } set {  projectileController = value; } }

    // Start is called before the first frame update
    void Start()
    {
        projectileController = GameObject.Find("ProjectileController").GetComponent<ProjectileController>();
    }

    // Update is called once per frame 
    void Update()
    {
        
    }

    public void AddSpellToProjectileManager(Spell spell, int slot)
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectNames.SpellSlotted);

        if (slot == 0)
        {
            projectileController.TwoSecSlot = spell;
            projectileController.InstantiateManagers(spell, 1.0f);
        }
        else if (slot == 1)
        {
            projectileController.FiveSecSlot = spell;
            projectileController.InstantiateManagers(spell, 3.0f);
        }
        else if (slot == 2)
        {
            projectileController.TenSecSlot = spell;
            projectileController.InstantiateManagers(spell, 8.0f);
        }
    }
}
