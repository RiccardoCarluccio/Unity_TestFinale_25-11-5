using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Gestione del Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MusicManager creato e reso persistente");
        }
        else
        {
            // Se esiste già, distruggi questo duplicato
            Debug.Log("MusicManager duplicato trovato - distruzione in corso");
            Destroy(gameObject);
            return;
        }

        

        // Ottieni l'AudioSource
        InitializeAudioSource();
    }

    // Funzione per inizializzare/verificare l'AudioSource
    private void InitializeAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                Debug.LogError("AudioSource mancante sul MusicManager! Aggiungilo manualmente.");
            }
            else
            {
                Debug.Log("AudioSource trovato e inizializzato");
            }
        }
    }

    // Verifica che l'AudioSource sia valido prima di ogni operazione
    private bool CheckAudioSource()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource è null - tentativo di reinizializzazione...");
            InitializeAudioSource();
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource ancora null dopo reinizializzazione!");
            return false;
        }

        return true;
    }

    // Funzione per mutare/smutare l'audio
    public void ToggleMute()
    {
        if (!CheckAudioSource()) return;

        audioSource.mute = !audioSource.mute;
        Debug.Log("Audio mute: " + audioSource.mute);
    }

    // Funzione per impostare il volume (da 0 a 1)
    public void SetVolume(float volume)
    {
        if (!CheckAudioSource()) return;

        audioSource.volume = Mathf.Clamp01(volume); // Assicura che sia tra 0 e 1
        Debug.Log("Volume impostato a: " + audioSource.volume);
    }

    // Funzione per ottenere il volume attuale
    public float GetVolume()
    {
        if (!CheckAudioSource()) return 0.5f; // Valore di default se fallisce

        return audioSource.volume;
    }

    // Funzione per verificare se è mutato
    public bool IsMuted()
    {
        if (!CheckAudioSource()) return false;

        return audioSource.mute;
    }

    // Funzione per pausare la musica
    public void PauseMusic()
    {
        if (!CheckAudioSource()) return;

        audioSource.Pause();
        Debug.Log("Musica in pausa");
    }

    // Funzione per riprendere la musica
    public void PlayMusic()
    {
        if (!CheckAudioSource()) return;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Musica in riproduzione");
        }
    }

    // Funzione per cambiare la musica
    public void ChangeMusic(AudioClip newMusic)
    {
        if (!CheckAudioSource()) return;

        if (audioSource.clip == newMusic)
            return;

        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();
        Debug.Log("Musica cambiata a: " + newMusic.name);
    }
}
