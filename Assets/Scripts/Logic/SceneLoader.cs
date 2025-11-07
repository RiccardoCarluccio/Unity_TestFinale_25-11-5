using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button _crudButton;
    [SerializeField] private Button _surfButton;
    [SerializeField] private Button _gamesButton;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void OnEnable()
    {
        _crudButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Crud));
        _surfButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Surf));
        _gamesButton.onClick.AddListener(() => LoadChosenScene(ChosenScene.Scene_Videogames));
    }

    private void OnDisable()
    {
        _crudButton.onClick.RemoveAllListeners();
        _surfButton.onClick.RemoveAllListeners();
        _gamesButton.onClick.RemoveAllListeners();
    }

    private void LoadChosenScene(Enum scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }    
}
