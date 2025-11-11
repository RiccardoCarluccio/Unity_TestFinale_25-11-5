using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardUser : MonoBehaviour
{
    [Header("Dashboard")]
    [SerializeField] private Manager_User _userManager;
    [SerializeField] private CanvasGroup _certificatesGroup;
    [SerializeField] private GameObject _certificatesPanel;
    [SerializeField] private TextMeshProUGUI _nickname;
    [SerializeField] private Button _certificateButton, _logout, _closeCertificatesButton;

    [Header("Certificate UI Elements")]
    [SerializeField] private GameObject _sqlCertificateIcon;
    [SerializeField] private GameObject _surfCertificateIcon;
    [SerializeField] private GameObject _videogamesCertificateIcon;
    [SerializeField] private TextMeshProUGUI _certificatesStatusText;

    [Header("Colors")]
    [SerializeField] private Color _completedColor = Color.green;
    [SerializeField] private Color _notCompletedColor = Color.red;

    private bool enableDebugLogs = true;

    private void Start()
    {
        // Configura i listener dei pulsanti
        _certificateButton.onClick.AddListener(OnCertificateButtonClicked);
        _logout.onClick.AddListener(OnLogoutClicked);
        _closeCertificatesButton.onClick.AddListener(OnCloseCertificatesClicked);

        // Nascondi il pannello certificati all'inizio
        _certificatesPanel.SetActive(false);

        // Carica i dati dell'utente loggato
        LoadUserData();
    }

    private void OnEnable()
    {
        // Ricarica i dati ogni volta che la dashboard diventa attiva
        LoadUserData();
    }

    /// <summary>
    /// Carica i dati dell'utente loggato e aggiorna la UI
    /// </summary>
    private void LoadUserData()
    {
        if (LoggedUser.Instance == null || LoggedUser.Instance.User == null)
        {
            if (enableDebugLogs)
                Debug.LogWarning("Nessun utente loggato trovato");
            
            _nickname.text = "Ospite";
            return;
        }

        User currentUser = LoggedUser.Instance.User;
        _nickname.text = currentUser.nickname;

        if (enableDebugLogs)
            Debug.Log($"Caricati dati utente: {currentUser.nickname}");
    }

    /// <summary>
    /// Chiamato quando si clicca sul pulsante dei certificati
    /// </summary>
    private void OnCertificateButtonClicked()
    {
        if (LoggedUser.Instance == null || LoggedUser.Instance.User == null)
        {
            if (enableDebugLogs)
                Debug.LogWarning("Nessun utente loggato");
            return;
        }

        // Mostra il pannello certificati
        _certificatesPanel.SetActive(true);
        StartCoroutine(FadeInPanel(_certificatesGroup));

        // Aggiorna la visualizzazione dei certificati
        UpdateCertificatesDisplay();
    }

    /// <summary>
    /// Aggiorna la visualizzazione dei certificati in base allo stato dell'utente
    /// </summary>
    private void UpdateCertificatesDisplay()
    {
        User currentUser = LoggedUser.Instance.User;
        
        // Ottieni lo stato dei certificati (potrebbero essere null se non ancora inizializzati)
        CertificatesData userCertificates = currentUser.certificates;

        if (enableDebugLogs)
        {
            if (userCertificates != null)
                Debug.Log($"Certificati utente - SQL: {userCertificates.sql}, Surf: {userCertificates.surf}, Videogames: {userCertificates.videogames}");
            else
                Debug.Log("Certificati utente: null");
        }

        // Aggiorna ogni certificato
        UpdateCertificateIcon(_sqlCertificateIcon, HasCertificate(Certificates.Sql), "SQL");
        UpdateCertificateIcon(_surfCertificateIcon, HasCertificate(Certificates.Surf), "Surf");
        UpdateCertificateIcon(_videogamesCertificateIcon, HasCertificate(Certificates.Videogames), "Videogames");

        // Aggiorna il testo di stato
        UpdateCertificatesStatusText();
    }
    /// <summary>
    /// Verifica se l'utente ha completato un certificato specifico
    /// </summary>
    private bool HasCertificate(Certificates certificate)
    {
        User currentUser = LoggedUser.Instance.User;
        
        // Se i certificati non sono stati inizializzati, ritorna false
        if (currentUser.certificates == null)
        {
            if (enableDebugLogs)
                Debug.LogWarning("Certificati non inizializzati per l'utente");
            return false;
        }
        
        return currentUser.certificates.HasCertificate(certificate);
    }

    /// <summary>
    /// Aggiorna l'aspetto di un'icona certificato
    /// </summary>
    private void UpdateCertificateIcon(GameObject certificateIcon, bool isCompleted, string certificateName)
    {
        if (certificateIcon == null) return;

        TextMeshProUGUI textColor = certificateIcon.GetComponent<TextMeshProUGUI>();
        if (textColor != null)
        {
            textColor.color = isCompleted ? _completedColor : _notCompletedColor;
        }

        // Opzionale: aggiungi un tooltip o testo
        TextMeshProUGUI certificateText = certificateIcon.GetComponentInChildren<TextMeshProUGUI>();
        if (certificateText != null)
        {
            certificateText.text = $"{certificateName}";
        }

        if (enableDebugLogs)
            Debug.Log($"Certificato {certificateName}: {(isCompleted ? "Completato" : "Non completato")}");
    }

    /// <summary>
    /// Aggiorna il testo di stato generale dei certificati
    /// </summary>
    private void UpdateCertificatesStatusText()
    {
        int completedCount = 0;
        int totalCertificates = 3;

        if (HasCertificate(Certificates.Sql)) completedCount++;
        if (HasCertificate(Certificates.Surf)) completedCount++;
        if (HasCertificate(Certificates.Videogames)) completedCount++;

        if (_certificatesStatusText != null)
        {
            _certificatesStatusText.text = $"Certificati completati: {completedCount}/{totalCertificates}";
        }
    }

    /// <summary>
    /// Chiude il pannello dei certificati
    /// </summary>
    private void OnCloseCertificatesClicked()
    {
        StartCoroutine(FadeOutPanel(_certificatesGroup));
    }

    /// <summary>
    /// Gestisce il logout
    /// </summary>
    private void OnLogoutClicked()
    {
        if (LoggedUser.Instance != null)
        {
            LoggedUser.Instance.User = null;
        }

        if (enableDebugLogs)
            Debug.Log("Utente disconnesso");

        // Torna alla scena di login
        SceneManager.LoadScene("Scene_Login_Register"); // Modifica con il nome della tua scena di login
    }

    /// <summary>
    /// Fade in del pannello certificati
    /// </summary>
    private IEnumerator FadeInPanel(CanvasGroup canvasGroup)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// Fade out del pannello certificati
    /// </summary>
    private IEnumerator FadeOutPanel(CanvasGroup canvasGroup)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        _certificatesPanel.SetActive(false);
    }
}