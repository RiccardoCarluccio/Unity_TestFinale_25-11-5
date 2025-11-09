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
    [SerializeField] private Button _loginButton, _createUserButton;

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
    }

    private void OnDisable()
    {
        _userManager.OnUserLoaded -= LogUser;
    }

    private void Start()
    {
        _loginButton.onClick.AddListener(OnLoginButtonClicked);
        _disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";
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
        ;

        _userManager.LoadUserByNicknameAndPassword(nickname, password);
    }

    private void OnDisconnectButtonClicked()
    {
        _user = null;
        _userImage.color = Color.red;
        _nicknameText.text = "Ospite";
    }
}
