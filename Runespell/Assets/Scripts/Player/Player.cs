using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    TMPro.TextMeshPro textMeshPro;
    [SerializeField]
    private float width = 0.1f;
    [SerializeField]
    private float height = 0.1f;
    [SerializeField] private float health = 100;
    [SerializeField] float lastHit = 0;
    [SerializeField] float damageCooldown = 0.5f;
    [SerializeField] private string loseScene;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private float damageEffectTime;

    private float maxHealth = 100;

    // Health Bar

    [SerializeField]
    private GameObject healthBar;

    private RectTransform healthBarTransform;

    [SerializeField]
    private float maxHealthBarHeight = 100.0f;

    [SerializeField]
    private float maxHealthBarWidth = 100.0f;


    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = health;
        healthBarTransform = healthBar.GetComponent<RectTransform>();
        SetHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - damageEffectTime > 0.2f)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //Deals damage to the player
    public void TakeDamage(float amount)
    {
        if (Time.time-lastHit>damageCooldown)
        {
            if (health - amount <= 0.0f)
            {
                SoundManager.instance.PlaySoundEffect(SoundEffectNames.PlayerDeath);
            }
            else
            {
                SoundManager.instance.PlaySoundEffect(SoundEffectNames.PlayerHurt);
            }


            lastHit = Time.time;
            health -= amount;
            healthText.text = "Health: " + health;
            SetHealthUI();
            damageEffectTime = Time.time;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    //Used for the Heal Spell
    public void Heal(float amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        SetHealthUI();
        healthText.text = "Health: " + health;
    }

    public void SetHealthUI()
    {
        healthText.text = "Health: " + health;
        float newHeight = Mathf.Lerp(0.0f, maxHealthBarHeight, health / maxHealth);
        healthBarTransform.sizeDelta = new Vector2(maxHealthBarWidth, newHeight);
    }

    //Get and set statements
    public float Width { get { return width; } }
    public float Height { get { return height; } }
    public float Health { get { return health; } set { health = value; } }
    public float MaxHealth { get { return maxHealth; } }
}
