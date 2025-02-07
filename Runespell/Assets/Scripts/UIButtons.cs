using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField] Object sceneToSwitchTo;
    [SerializeField] private string sceneName;
    public UnityEngine.UI.Button sceneButton;

    void Start()
    {
        UnityEngine.UI.Button button = sceneButton.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(SwitchScene);
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
