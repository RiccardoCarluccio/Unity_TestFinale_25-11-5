using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;

    public static SceneLoader Instance { get { return _instance; } }

    public int category_id;

    [SerializeField] private Button _crudButton;
    [SerializeField] private Button _surfButton;
    [SerializeField] private Button _gamesButton;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

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
        category_id = Convert.ToInt32(scene) + 1;
        if (enableDebugLogs)
            Debug.Log(category_id);
        SceneManager.LoadScene(scene.ToString());
    }

    private enum ChosenScene
    {
        Scene_Crud,
        Scene_Surf,
        Scene_Videogames,
    }
}
