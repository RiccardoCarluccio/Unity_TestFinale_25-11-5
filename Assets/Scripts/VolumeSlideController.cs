using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    private Slider volumeSlider;
    private MusicManager musicManager;
    private bool isInitialized = false;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        // Cerca il MusicManager
        musicManager = FindFirstObjectByType<MusicManager>();

        if (musicManager == null)
        {
            Debug.LogWarning("MusicManager non trovato. Lo slider del volume sarà disabilitato.");
            volumeSlider.interactable = false;
            return;
        }

        // Imposta valore iniziale
        volumeSlider.value = musicManager.GetVolume();

        // Collega lo slider
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        isInitialized = true;

        Debug.Log("Slider Volume collegato al MusicManager - Volume iniziale: " + volumeSlider.value);
    }

    void OnVolumeChanged(float value)
    {
        if (musicManager != null && isInitialized)
        {
            musicManager.SetVolume(value);
        }
    }
}
