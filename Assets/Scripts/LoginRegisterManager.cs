using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginRegisterManager : MonoBehaviour
{
    [Header("Pannelli UI")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject modificaUtentePanel;

    [Header("Login")]
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public Button loginButton;
    public Button switchToRegisterButton;
    public Button editButton; // Bottone per aprire il panel Modifica Utente

    // Register 
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerEmailInput; 
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;
    public Button registerButton;
    public Button switchToLoginButton;

    // Modifica Utente 
    public TMP_InputField editUsernameInput;
    public TMP_InputField editNewUsernameInput;
    public TMP_InputField editPasswordInput;
    public TMP_InputField editNewPasswordInput;
    public Button confirmEditButton;
    public Button backToLoginButton;

    // Messaggi di feedback
    public TextMeshProUGUI feedbackText;

    void Start()
    {
        // Imposta pannelli iniziali
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        modificaUtentePanel.SetActive(false);
        feedbackText.text = "";

        // Collega i bottoni di Login/Registrazione
        loginButton.onClick.AddListener(OnLoginClicked);
        switchToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        registerButton.onClick.AddListener(OnRegisterClicked);
        switchToLoginButton.onClick.AddListener(ShowLoginPanel);

        // Collega bottone per Modifica Utente
        editButton.onClick.AddListener(ShowEditPanel);
        confirmEditButton.onClick.AddListener(OnEditConfirm);
        backToLoginButton.onClick.AddListener(ShowLoginPanel);
    }

    // Funzioni per mostrare i pannelli

    void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        modificaUtentePanel.SetActive(false);
        feedbackText.text = "";
    }

    void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        modificaUtentePanel.SetActive(false);
        feedbackText.text = "";
    }

    void ShowEditPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        modificaUtentePanel.SetActive(true);
        feedbackText.text = "";
    }

    // Gestione Login

    void OnLoginClicked()
    {
        string username = loginUsernameInput.text.Trim();
        string password = loginPasswordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Inserisci username e password";
            return;
        }

        // TODO: inserisci qui la verifica reale
        bool success = FakeLoginCheck(username, password);

        feedbackText.text = success ? "Login riuscito" : "Username o password errati";
    }

    // Gestione Registrazione

    void OnRegisterClicked()
    {
        string username = registerUsernameInput.text.Trim();
        string email = registerEmailInput.text.Trim();
        string password = registerPasswordInput.text.Trim();
        string confirmPassword = registerConfirmPasswordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Compila tutti i campi";
            return;
        }

        if (password != confirmPassword)
        {
            feedbackText.text = "Le password non coincidono";
            return;
        }

        // TODO: inserisci qui la registrazione reale
        bool success = FakeRegistration(username, email, password);

        feedbackText.text = success ? "Registrazione riuscita! Effettua il login" : "Errore durante la registrazione";
    }

    // Gestione Modifica Utente

    void OnEditConfirm()
    {
        string username = editUsernameInput.text.Trim();
        string newUsername = editNewUsernameInput.text.Trim();
        string password = editPasswordInput.text.Trim();
        string newPassword = editNewPasswordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Inserisci username e password attuali";
            return;
        }

        // TODO: inserisci qui la modifica reale del profilo
        bool success = FakeEditUser(username, password, newUsername, newPassword);

        feedbackText.text = success ? "Dati modificati con successo!" : "Errore. Controlla i dati inseriti.";
    }

    // Mock methods di esempio (da sostituire con database )

    bool FakeLoginCheck(string username, string password)
    {
        return username == "user" && password == "pass";
    }

    bool FakeRegistration(string username, string email, string password)
    {
        return true;
    }

    bool FakeEditUser(string username, string password, string newUsername, string newPassword)
    {
        return true;
    }
}
