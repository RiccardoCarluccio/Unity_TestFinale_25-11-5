using UnityEngine;
using UnityEngine.UI;
using TMPro; // Se usi TextMeshPro

public class MuteButtonController : MonoBehaviour
{
    private Button muteButton;
    private MusicManager musicManager;
    private TextMeshProUGUI buttonText; // O Text se usi UI Text normale

    void Start()
    {
        muteButton = GetComponent<Button>();

        // Cerca il testo del bottone (se c'è)
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        // Se usi UI Text normale invece di TextMeshPro, usa:
        // buttonText = GetComponentInChildren<Text>();

        // Cerca il MusicManager
        musicManager = FindAnyObjectByType<MusicManager>();

        if (musicManager == null)
        {
            Debug.LogWarning("MusicManager non trovato. Il bottone Mute sarà disabilitato.");
            muteButton.interactable = false;
            return;
        }

        // Collega il bottone
        muteButton.onClick.RemoveAllListeners();
        muteButton.onClick.AddListener(OnMuteButtonClick);

        // Aggiorna lo stato visivo iniziale
        UpdateButtonVisual();

        Debug.Log("Bottone Mute collegato al MusicManager");
    }

    void OnMuteButtonClick()
    {
        if (musicManager != null)
        {
            musicManager.ToggleMute();
            UpdateButtonVisual();
        }
    }

    void UpdateButtonVisual()
    {
        if (musicManager == null || buttonText == null) return;

        // Cambia il testo in base allo stato del mute
        if (musicManager.IsMuted())
        {
            buttonText.text = "UNMUTE"; // Audio disattivato
        }
        else
        {
            buttonText.text = "MUTE"; // Audio attivo
        }
    }
}
