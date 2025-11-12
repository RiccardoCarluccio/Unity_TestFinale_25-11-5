using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDashboard : MonoBehaviour
{
    [SerializeField] private Manager_User _userManager;
    [SerializeField] private GameObject _trophyPanel;
    [SerializeField] private Button _openAndCloseDashboardButton;
    [SerializeField] private Button _closeDashboardButton;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private TextMeshProUGUI _trophyPrefab;

    private void Start()
    {
        _openAndCloseDashboardButton.onClick.AddListener(OpenCloseDashboard);
        _closeDashboardButton.onClick.AddListener(CloseDashboard);

        _userManager.OnCertificatesLoaded += HandleCertificatesLoaded;

        _trophyPanel.SetActive(false);
    }

    private void OnDisable()
    {
        _openAndCloseDashboardButton.onClick.RemoveAllListeners();
        _closeDashboardButton.onClick.RemoveAllListeners();

        _userManager.OnCertificatesLoaded -= HandleCertificatesLoaded;
    }

    private void OpenCloseDashboard()
    {
        bool willOpen = !_trophyPanel.activeSelf;
        _trophyPanel.SetActive(willOpen);

        _userManager.GetCertificatesByNickname(LoggedUser.Instance.User.nickname);      //add condition to check if the user is logged?
    }

    private void CloseDashboard()       //possibly not working because it will be assigned before the button being enabled. In that case, move the method in a button script
    {
        _trophyPanel?.SetActive(false);
    }

    private void HandleCertificatesLoaded(CertificatesAsObjects certificates)
    {
        DisplayCertificates(certificates);
    }

    private void DisplayCertificates(CertificatesAsObjects certificates)
    {
        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        AddTrophy("Sql", certificates.sql);
        AddTrophy("Surf", certificates.surf);
        AddTrophy("Videogames", certificates.videogames);
    }

    private void AddTrophy(string name, bool completed)
    {
        var trophyText = Instantiate(_trophyPrefab, _contentParent);
        trophyText.text = name;
        trophyText.color = completed ? Color.green : Color.red;
    }    
}