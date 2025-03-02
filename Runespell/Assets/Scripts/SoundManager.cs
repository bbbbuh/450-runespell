using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// In Global Space
public enum SoundEffectNames
{
    EnemyHurt
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
            default: return null;
        }
    }


    public void PlaySoundEffect(Transform spawnTransform, SoundEffectNames name)
    {
        // Spawns sound holding GameObject
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

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
