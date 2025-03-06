using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    PlayerWalk,
    Door,
    SpellSlotted,
    Fireball,
    Heal,
    MagicOrb,
    MagicOrbExplosion
}

public enum SongNames
{
    Battle,
    Calm
}

// What are the differences between these two?
// GameState changes the current mood of the game
// So for example, if all the enemies are dead,
// the game will be in a calm state, and the
// calm song will play
public enum GameState
{
    Battle,
    Calm
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

    [SerializeField]
    private AudioSource songObject;

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
    private AudioClip playerWalk;

    [SerializeField]
    private AudioClip door;

    [SerializeField]
    private AudioClip spellSlotted;

    [SerializeField]
    private AudioClip fireball;

    [SerializeField]
    private AudioClip heal;

    [SerializeField]
    private AudioClip magicOrb;

    [SerializeField]
    private AudioClip magicOrbExplosion;

    // End of Sound Effects



    // Songs

    [SerializeField]
    private AudioClip battle;

    [SerializeField]
    private AudioClip calm;

    // End of Songs



    [SerializeField]
    private AudioSource battleMusic_Battle;

    [SerializeField]
    private AudioSource battleMusic_Calm;

    [SerializeField]
    private AudioSource walk;


    private GameState currentGameState = GameState.Calm;

    public GameState CurrentGameState { get  { return currentGameState; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        // For now, since we just have one song, we'll use this
        // SoundManager.instance.PlaySong(SongNames.Battle);

        SetUpBattleSongs();
        SetUpWalkAudio();
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
            case SoundEffectNames.PlayerWalk: return playerWalk;
            case SoundEffectNames.Door: return door;
            case SoundEffectNames.SpellSlotted: return spellSlotted;
            case SoundEffectNames.Fireball: return fireball;
            case SoundEffectNames.Heal: return heal;
            case SoundEffectNames.MagicOrb: return magicOrb;
            case SoundEffectNames.MagicOrbExplosion: return magicOrbExplosion;
            default: return null;
        }
    }

    private AudioClip GetSong(SongNames name)
    {
        switch (name)
        {
            case SongNames.Battle: return battle;
            case SongNames.Calm: return calm;
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

    public void PlaySong(SongNames name)
    {
        // Spawns sound holding GameObject
        AudioSource audioSource = Instantiate(songObject, new Vector3(0, 0, 0), Quaternion.identity);

        // Gets and gives the GameObject the AudioClip
        audioSource.clip = GetSong(name);

        // Adjusts volume
        audioSource.volume = masterVolume * songVolume;

        // Plays sound
        audioSource.Play();

        // Prevents the GameObject from being destroyed
        DontDestroyOnLoad(audioSource);
    }

    // Functions for setting up changing battle music

    private void SetUpBattleSongs()
    {
        // Spawns sound holding GameObject
        battleMusic_Battle = Instantiate(songObject, new Vector3(0, 0, 0), Quaternion.identity);
        battleMusic_Calm = Instantiate(songObject, new Vector3(0, 0, 0), Quaternion.identity);

        // Gets and gives the GameObject the AudioClip
        battleMusic_Battle.clip = GetSong(SongNames.Battle);
        battleMusic_Calm.clip = GetSong(SongNames.Calm);

        // Adjusts volume to be 0 initially
        battleMusic_Battle.volume = 0;
        battleMusic_Calm.volume = 0;

        // Plays the song in the background
        battleMusic_Battle.Play();
        battleMusic_Calm.Play();

        // Prevents the GameObjects from being destroyed
        DontDestroyOnLoad(battleMusic_Battle);
        DontDestroyOnLoad(battleMusic_Calm);

        SwitchSong(GameState.Calm);
    }

    private IEnumerator FadeSong(AudioSource audioSource, float targetVolume, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void SwitchSong(GameState name)
    {
        currentGameState = name;        
        UnityEngine.Debug.Log("GAMESTATE: " + currentGameState);

        float fadeDuration = 3f;  // Fade duration in seconds

        switch (name)
        {
            case GameState.Battle:
                StartCoroutine(FadeSong(battleMusic_Calm, 0f, fadeDuration)); 
                StartCoroutine(FadeSong(battleMusic_Battle, masterVolume * songVolume, fadeDuration));
                break;
            case GameState.Calm:
                StartCoroutine(FadeSong(battleMusic_Battle, 0f, fadeDuration)); 
                StartCoroutine(FadeSong(battleMusic_Calm, masterVolume * songVolume, fadeDuration));  
                break;
            default:
                break;
        }
    }

    // Walking Audio Effects

    private void SetUpWalkAudio()
    {
        // Spawns sound holding GameObject
        walk = Instantiate(songObject, new Vector3(0, 0, 0), Quaternion.identity);

        // Gets and gives the GameObject the AudioClip
        walk.clip = GetSoundEffect(SoundEffectNames.PlayerWalk);

        // Adjusts volume 
        walk.volume = soundEffectVolume * masterVolume;

        // Prevents the GameObjects from being destroyed
        DontDestroyOnLoad(walk);
    }

    public void PlayWalk() 
    {
        if (!walk.isPlaying)
        {
            walk.Play();
        }
    }

    public void StopWalk() {walk.Stop();}
}
