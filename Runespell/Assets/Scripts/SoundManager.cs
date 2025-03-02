using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// In Global Space
public enum SoundEffectNames
{
    EnemyHurt,
    EnemyDeath,
    PlayerHurt,
    PlayerDeath,
    PlayerDash,
    SpellSlotted,
    Fireball,
    Heal
}

public class SoundManager : MonoBehaviour
{
    // Adapted Sasquatch B Studio's How To Add Sound Effects The Right Way guide
    // This is a singleton

    public static SoundManager instance;

    [SerializeField]
    float masterVolume = 1.0f;

    [SerializeField]
    float songVolume = 1.0f;

    [SerializeField]
    float soundEffectVolume = 1.0f;

    [SerializeField]
    private AudioSource soundObject;

    // Sound Effects

    [SerializeField]
    private AudioClip enemyHurt;

    [SerializeField]
    private AudioClip enemyDeath;

    [SerializeField]
    private AudioClip playerHurt;

    [SerializeField]
    private AudioClip playerDeath;

    [SerializeField]
    private AudioClip playerDash;

    [SerializeField]
    private AudioClip spellSlotted;

    [SerializeField]
    private AudioClip fireball;

    [SerializeField]
    private AudioClip heal;

    // End of Sound Effects

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private AudioClip GetSoundEffect(SoundEffectNames name)
    {
        switch (name)
        {
            case SoundEffectNames.EnemyHurt: return enemyHurt;
            case SoundEffectNames.EnemyDeath: return enemyDeath;
            case SoundEffectNames.PlayerHurt: return playerHurt;
            case SoundEffectNames.PlayerDeath: return playerDeath;
            case SoundEffectNames.PlayerDash: return playerDash;
            case SoundEffectNames.SpellSlotted: return spellSlotted;
            case SoundEffectNames.Fireball: return fireball;
            case SoundEffectNames.Heal: return heal;
            default: return null;
        }
    }


    public void PlaySoundEffect(SoundEffectNames name)
    {
        // Spawns sound holding GameObject
        AudioSource audioSource = Instantiate(soundObject, new Vector3(0, 0, 0), Quaternion.identity);

        // Gets and gives the GameObject the AudioClip
        audioSource.clip = GetSoundEffect(name);

        // Adjusts volume
        audioSource.volume = masterVolume * soundEffectVolume;

        // Plays sound
        audioSource.Play();

        // Get length of clip
        float clipLength = audioSource.clip.length;

        // Deletes GameObject after the clip is done
        Destroy(audioSource.gameObject, clipLength);
    }
}
