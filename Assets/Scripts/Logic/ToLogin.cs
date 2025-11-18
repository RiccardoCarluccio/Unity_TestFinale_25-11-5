using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToLogin : MonoBehaviour
{
    [SerializeField] private Button _toLoginButton;

    private void OnEnable()
    {
        _toLoginButton.onClick.AddListener(OnLogout);
    }

    private void OnDisable()
    {
        _toLoginButton.onClick.RemoveAllListeners();
    }

    private void OnLogout()
    {
        LoggedUser.Instance.Logout();
        SceneManager.LoadScene("Scene_Login_Register");
    }
}
