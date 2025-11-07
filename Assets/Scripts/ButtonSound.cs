using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Button component mancante su " + gameObject.name);
            return;
        }

        // Aggiungi listener per il click
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Riproduci il suono tramite il SoundManager
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClick();
        }
    }

    // suono quando il mouse passa sopra il bottone
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonHover();
        }
    }
}
