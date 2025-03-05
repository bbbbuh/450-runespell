using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealSpell : Spell
{
    private float baseHealAmount = 1.0f;

    [SerializeField] GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public override void Fire()
    {        
        if (this.Player.Health < this.Player.MaxHealth)
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectNames.Heal);

            GameObject explosion = Instantiate(explosionPrefab, Player.transform.position, Quaternion.identity);
            explosion.GetComponent<HealExplosion>().SpawnTime = Time.time;
        }

        this.Player.Heal(baseHealAmount * this.Multiplier);

        //UnityEngine.Debug.Log("Player healed for: " + (baseHealAmount * this.Multiplier) + " amount");
        //UnityEngine.Debug.Log("Current player health is: " + this.Player.Health);
    }

    public override void SetProjectileMult(float mult)
    {

    }

    public override SpellNames GetSpellName()
    {
        return SpellNames.Heal;
    }
}
