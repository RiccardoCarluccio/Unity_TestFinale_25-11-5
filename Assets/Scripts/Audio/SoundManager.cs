using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("UI Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip buttonHoverSound; 
    
    [Header("Game Sounds")]
    public AudioClip correctAnswerSound; // Per risposte corrette
    public AudioClip wrongAnswerSound; // Per risposte sbagliate

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource mancante sul SoundManager!");
        }
    }

    // Funzione per riprodurre il suono del click
    public void PlayButtonClick()
    {
        PlaySound(buttonClickSound);
    }

    // Funzione per riprodurre il suono hover (opzionale)
    public void PlayButtonHover()
    {
        PlaySound(buttonHoverSound);
    }

    // Funzione per risposta corretta
    public void PlayCorrectAnswer()
    {
        PlaySound(correctAnswerSound);
    }

    // Funzione per risposta sbagliata
    public void PlayWrongAnswer()
    {
        PlaySound(wrongAnswerSound);
    }

    // Funzione generica per riprodurre un suono
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip o AudioSource mancante!");
        }
    }

    // Funzione per riprodurre un suono con volume specifico
    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}

