using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button _sqlButton;
    [SerializeField] private Button _surfButton;
    [SerializeField] private Button _gamesButton;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void OnEnable()
    {
        _sqlButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Sql));
        _surfButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Surf));
        _gamesButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Videogames));
    }

    private void OnDisable()
    {
        _sqlButton.onClick.RemoveAllListeners();
        _surfButton.onClick.RemoveAllListeners();
        _gamesButton.onClick.RemoveAllListeners();
    }

    private void LoadChosenScene(Enum scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }    
}
