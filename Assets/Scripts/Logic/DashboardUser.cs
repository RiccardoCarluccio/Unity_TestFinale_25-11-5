using System;
using System.Collections;
using TMPro;
using UnityEditor.SearchService;
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
    private Certificates _certificatesText;

    private bool enableDebugLogs = true;

    public float fadeDuration = 0.3f;

    private void OnEnable()
    {
        _userManager.OnCertificatesLoaded += LoadCertificates;
    }

    private void OnDisable()
    {
        _userManager.OnUserLoaded -= LoadCertificates;
    }

    void Start()
    {
        _logout.onClick.AddListener(OnLogoutClicked);
        _certificateButton.onClick.AddListener(OnCertificateClicked);

        _certificatesGroup.alpha = 0;
        _certificatesPanel.SetActive(false);

        if (_closeCertificatesButton != null)
        {
            _closeCertificatesButton.onClick.AddListener(HideCertificatesPanel);
        }
    }

    void Awake()
    {
        _nickname.text = LoggedUser.Instance.User.nickname;
    }

    private void OnLogoutClicked()
    {
        LoggedUser.Instance.Logout();

        if (enableDebugLogs)
            Debug.Log("Disconnessione avvenuta con successo!");

        SceneManager.LoadScene("Scene_Login_Register");
    }
    
    private void LoadCertificates(User user)
    {
        string nickname = user.nickname;
        _certificatesText = user.certificates;

        if (string.IsNullOrEmpty(nickname))
        {
            if (enableDebugLogs)
            {
                Debug.Log($"No certificates found for user: {nickname}");
            }
        }
        
        _userManager.GetCertificates(nickname, _certificatesText);
    }

    private void OnCertificateClicked()
    {
        _certificatesPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(_certificatesGroup, 0, 1, fadeDuration));
    }

    private void HideCertificatesPanel()
    {
        StartCoroutine(HideAndDeactivate());
    }

    IEnumerator HideAndDeactivate()
    {
        yield return FadeCanvasGroup(_certificatesGroup, 1, 0, fadeDuration);
        _certificatesPanel.SetActive(false);
    }
    
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
