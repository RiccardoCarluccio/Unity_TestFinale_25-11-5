using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager_LoggedUser : MonoBehaviour
{
    private static Manager_LoggedUser _instance;
    public static Manager_LoggedUser Instance { get { return _instance; } }

    [Header("Login Panel")]
    [SerializeField] private Manager_User _userManager;
    [SerializeField] private TMP_InputField _nicknameField, _passwordField;
    [SerializeField] private Button _loginButton, _createUserButton, _deleteUserButton;
    [SerializeField] private TextMeshProUGUI _userActionUpdate;

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
            _instance = this;
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

        _disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";
    }

    public void DisplayError(string message)
    {
        _userActionUpdate.color = Color.red;
        _userActionUpdate.text = "Error";

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
        _userActionUpdate.color = Color.black;
        _userActionUpdate.text = "Login effettuato con successo!";

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
    }

    private void OnDisconnectButtonClicked()
    {
        _user = null;
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";

        _userActionUpdate.color = Color.black;
        _userActionUpdate.text = "Disconnessione effettuata con successo!";
    }

    private void OnCreateUserClicked()
    {
        string nickname = _nicknameField.text.Trim();
        string password = _passwordField.text.Trim();

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Missing nickname and/or password");
            return;
        }

        _userManager.CreateUser(nickname, password);        
    }

    private void UserCreated(User user)
    {
        _userActionUpdate.color = Color.black;
        _userActionUpdate.text = "Utente creato con successo!";
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
        _userActionUpdate.color = Color.black;
        _userActionUpdate.text = "Utente eliminato con successo!";
    }    
}
