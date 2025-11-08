using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_LoggedUser : MonoBehaviour
{
    private static Manager_LoggedUser _instance;
    public static Manager_LoggedUser Instance { get { return _instance; } }

    [SerializeField] private Manager_User _userManager;
    [SerializeField] private TMP_InputField _nicknameField, _passwordField;
    [SerializeField] private Button _loginButton, _createUserButton;

    private User _user;

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
        //_userManager.OnUserLoaded += ;
    }

    private void OnDisable()
    {
        //_userManager.OnUserLoaded -= ;
    }

    private void GetUserByNicknameAndPassword(User user)
    {
        
    }

    private void LogUser(User user)
    {
        if (user is null)
        {
            if (enableDebugLogs)
                Debug.LogError($"");
            return;
        }

        _user = user;
    }
}
