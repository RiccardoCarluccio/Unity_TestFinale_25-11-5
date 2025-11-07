using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    // Riferimenti
    public GameObject transitionPanel;
    public Animator transitionAnimator;
    public float transitionDuration = 0.5f;

    // Variabile per assicurarci che il pannello si disattivi
    private bool isTransitioning = false;

    void Start()
    {
        // All'inizio di ogni scena, esegui il FadeOut
        if (transitionPanel != null)
        {
            // ASSICURATI che il pannello sia attivo per fare il FadeOut
            transitionPanel.SetActive(true);
            
            // Avvia il FadeOut
            transitionAnimator.SetTrigger("FadeOut");
            
            // Dopo il FadeOut, DISATTIVA il pannello
            StartCoroutine(DisablePanelAfterFadeOut());
        }
    }

    // Coroutine per disattivare il pannello dopo il FadeOut
    private IEnumerator DisablePanelAfterFadeOut()
    {
        // Aspetta che l'animazione FadeOut finisca
        yield return new WaitForSeconds(transitionDuration);
        
        // DISATTIVA il pannello così non blocca i click
        transitionPanel.SetActive(false);
        
        Debug.Log("TransitionPanel DISATTIVATO - UI ora cliccabile");
    }

    // Funzione pubblica per caricare una scena con transizione
    public void LoadSceneWithTransition(string sceneName)
    {
        // Evita che si avviino più transizioni contemporaneamente
        if (isTransitioning)
        {
            Debug.LogWarning("Transizione già in corso, aspetta...");
            return;
        }

        // Verifica che il canvas sia attivo
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("TransitionCanvas è disabilitato!");
            return;
        }

        StartCoroutine(TransitionToScene(sceneName));
    }

    // Coroutine per la transizione tra scene
    private IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true;
        
        // RIATTIVA il pannello per la transizione
        transitionPanel.SetActive(true);
        
        // Avvia il FadeIn (schermo diventa nero)
        transitionAnimator.SetTrigger("FadeIn");
        
        Debug.Log("Inizio FadeIn - Caricamento scena: " + sceneName);
        
        // Aspetta che il FadeIn finisca
        yield return new WaitForSeconds(transitionDuration);
        
        // Carica la nuova scena
        SceneManager.LoadScene(sceneName);
        
        
        isTransitioning = false;
    }
}
