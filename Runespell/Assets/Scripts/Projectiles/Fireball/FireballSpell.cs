using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireballSpell : Spell
{
    //projectile prefab
    [SerializeField]
    private FireballProjectile fireballProjectile;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Debug.Log("SPELL MULT: " + this.Multiplier);
        //SetProjectileMult(this.Multiplier);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public override void Fire()
    {
        closestTarget = projectileController.ClosestTarget;
        if (closestTarget != null)
        {
            Vector3 playerVec = player.transform.position;
            Vector3 targetVec = closestTarget.transform.position;
            playerVec.z = 1.0f;
            targetVec.z = 1.0f;
            Vector3 direction = (targetVec - playerVec).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Projectile proj = Instantiate(fireballProjectile, player.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            proj.Direction = direction;
            projectileController.Projectiles.Add(proj);
            proj.GetComponent<Projectile>().TimeCast = Time.time;
            SoundManager.instance.PlaySoundEffect(SoundEffectNames.Fireball);
        }
        
    }

    public override void SetProjectileMult(float mult)
    {
        fireballProjectile.Multiplier = mult;
    }

    public override SpellNames GetSpellName()
    {
        return fireballProjectile.SpellName;
    }
}
