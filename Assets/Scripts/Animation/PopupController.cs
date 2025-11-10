using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel;
    public CanvasGroup popupCanvasGroup;
    public Button openPopupButton;
    public Button closePopupButton;
    public TextMeshProUGUI popupText;

    public float fadeDuration = 0.3f;

    void Start()
    {
        // Assicura che il popup sia inizialmente nascosto
        popupCanvasGroup.alpha = 0;
        popupPanel.SetActive(false);

        // funzione di apertura al bottone
        openPopupButton.onClick.AddListener(ShowPopup);

        // funzione di chiusura al bottone 
        if (closePopupButton != null)
        {
            closePopupButton.onClick.AddListener(HidePopup);
        }
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(popupCanvasGroup, 0, 1, fadeDuration));
    }

    public void HidePopup()
    {
        StartCoroutine(HideAndDeactivate());
    }

    IEnumerator HideAndDeactivate()
    {
        yield return FadeCanvasGroup(popupCanvasGroup, 1, 0, fadeDuration);
        popupPanel.SetActive(false);
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
