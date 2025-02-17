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
        PlayerDeath();
        SceneTransition();
    }

    void PlayerDeath()
    {
        if (player.GetComponent<Player>().Health<=0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    void SceneTransition()
    {
        if (nextScene)
        {
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
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene().name;
        player = GameObject.Find("Player");
        nextScene = false;
    }

    public bool NextScene { get { return nextScene; } set { nextScene = value; } }
}
