using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToMainMenu : MonoBehaviour
{
    [SerializeField] private Button _toMainMenuButton;

    private void OnEnable()
    {
        _toMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Scene_MainMenu"));
    }

    private void OnDisable()
    {
        _toMainMenuButton.onClick.RemoveAllListeners();
    }
}
