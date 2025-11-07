using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour


{
    
    [Header("Pannelli")]
    public GameObject questionPanel;
    public GameObject wrongAnswerPanel;

    void Start()
    {
        // All'inizio mostra solo il question panel
        ShowQuestionPanel();
    }

    // Mostra il pannello domanda, nasconde gli altri
    public void ShowQuestionPanel()
    {
        questionPanel.SetActive(true);
        wrongAnswerPanel.SetActive(false);
    }

    // Mostra il pannello risposta sbagliata, nasconde gli altri
    public void ShowWrongAnswerPanel()
    {
        questionPanel.SetActive(false);
        wrongAnswerPanel.SetActive(true);
    }

    public void ShowWrongAnswerPanelWithFade()
{
    StartCoroutine(FadeTransition(questionPanel, wrongAnswerPanel));
}

public void ShowQuestionPanelWithFade()
{
    StartCoroutine(FadeTransition(wrongAnswerPanel, questionPanel));
}

IEnumerator FadeTransition(GameObject fromPanel, GameObject toPanel)
{
    CanvasGroup fromGroup = fromPanel.GetComponent<CanvasGroup>();
    CanvasGroup toGroup = toPanel.GetComponent<CanvasGroup>();

    // Fade out
    float t = 0;
    while (t < 0.3f)
    {
        t += Time.deltaTime;
        fromGroup.alpha = 1 - (t / 0.3f);
        yield return null;
    }

    fromPanel.SetActive(false);
    toPanel.SetActive(true);
    toGroup.alpha = 0;

    // Fade in
    t = 0;
    while (t < 0.3f)
    {
        t += Time.deltaTime;
        toGroup.alpha = t / 0.3f;
        yield return null;
    }
    
    toGroup.alpha = 1;
}
}
