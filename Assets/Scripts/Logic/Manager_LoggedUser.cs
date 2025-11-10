using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager_LoggedUser : MonoBehaviour
{
    private static Manager_LoggedUser _instance;
    public static Manager_LoggedUser Instance { get { return _instance; } }

    [Header("Login Panel")]
    [SerializeField] private Manager_User _userManager;
    [SerializeField] private TMP_InputField _nicknameField, _passwordField;
    [SerializeField] private Button _loginButton, _deleteUserButton, _showRegisterPanel;
    [SerializeField] private TextMeshProUGUI _userActionUpdate;
    [SerializeField] private GameObject _loginPanel;

    [Header("Register Panel")]
    [SerializeField] private GameObject _registerPanel;
    [SerializeField] private TMP_InputField _registerNicknameField, _registerPasswordField;
    [SerializeField] private Button _returnToLoginButton, _createUserButton;

    [Header("User Panel")]
    [SerializeField] private Image _userImage;
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private Button _disconnectButton;

    private User _user;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        _userManager.OnUserLoaded += LogUser;
        _userManager.OnUserCreated += UserCreated;
        _userManager.OnUserDeleted += UserDeleted;
        _userManager.OnError += DisplayError;
    }

    private void OnDisable()
    {
        _userManager.OnUserLoaded -= LogUser;
        _userManager.OnUserCreated -= UserCreated;
        _userManager.OnUserDeleted -= UserDeleted;
        _userManager.OnError -= DisplayError;
    }

    private void Start()
    {
        _loginButton.onClick.AddListener(OnLoginButtonClicked);
        _createUserButton.onClick.AddListener(OnCreateUserClicked);
        _deleteUserButton.onClick.AddListener(OnDeleteUserClicked);
        _showRegisterPanel.onClick.AddListener(OnRegisterButtonClicked);
        _returnToLoginButton.onClick.AddListener(OnRegisterPanelLoginButtonClicked);

        _disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";
    }

    public void DisplayError(string message)
    {
        _userActionUpdate.color = Color.red;
        _userActionUpdate.text = "Error";
        StartCoroutine(ActionUpdateTextDelay(_userActionUpdate));

        if (enableDebugLogs)
            Debug.LogError(message);
    }

    private void LogUser(User user)
    {
        if (user is null)
        {
            if (enableDebugLogs)
                Debug.LogError($"Invalid nickname or password ");
            return;
        }

        _user = user;
        _userActionUpdate.color = Color.white;
        _userActionUpdate.text = "Login effettuato con successo!";
        StartCoroutine(ActionUpdateTextDelay(_userActionUpdate));

        if (enableDebugLogs)
            Debug.Log($"Logged user: {user.nickname}");

        _userImage.color = Color.green;
        _nicknameText.text = user.nickname;
    }

    private void OnLoginButtonClicked()
    {
        string nickname = _nicknameField.text.Trim();
        string password = _passwordField.text.Trim();

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Missing nickname and/or password");
            return;
        }

        _userManager.LoadUserByNicknameAndPassword(nickname, password);
        SceneManager.LoadScene("Scene_MainMenu");
    }

    private void OnDisconnectButtonClicked()
    {
        _user = null;
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";

        _userActionUpdate.color = Color.white;
        _userActionUpdate.text = "Disconnessione effettuata con successo!";
        StartCoroutine(ActionUpdateTextDelay(_userActionUpdate));
    }

    private void OnRegisterButtonClicked()
    {
        ShowRegisterPanel();
    }

    private void OnRegisterPanelLoginButtonClicked()
    {
        ShowLoginPanel();
    }

    private void OnCreateUserClicked()
    {
        string nickname = _registerNicknameField.text.Trim();
        string password = _registerPasswordField.text.Trim();

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Missing nickname and/or password");
            return;
        }

        _userManager.CreateUser(nickname, password);
    }

    private void UserCreated(User user)
    {
        _userActionUpdate.color = Color.white;
        _userActionUpdate.text = "Utente creato con successo!";
        StartCoroutine(ActionUpdateTextDelay(_userActionUpdate));
    }

    private void OnDeleteUserClicked()
    {
        string nickname = _nicknameField.text.Trim();
        string password = _passwordField.text.Trim();

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Missing nickname and/or password");
            return;
        }

        _userManager.DeleteUser(nickname, password);
    }

    private void UserDeleted(User user)
    {
        _userActionUpdate.color = Color.white;
        _userActionUpdate.text = "Utente eliminato con successo!";
        StartCoroutine(ActionUpdateTextDelay(_userActionUpdate));
    }

    private void ShowRegisterPanel()
    {
        _loginPanel.SetActive(false);
        _registerPanel.SetActive(true);
    }

    private void ShowLoginPanel()
    {
        _loginPanel.SetActive(true);
        _registerPanel.SetActive(false);
    }

    private IEnumerator ActionUpdateTextDelay(TextMeshProUGUI _userActionUpdate)
    {
        yield return new WaitForSeconds(1.5f);
        _userActionUpdate.text = "";
    }
}
