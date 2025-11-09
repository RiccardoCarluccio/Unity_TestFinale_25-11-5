using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class ButtonHoverVideo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject videoBackground; // Oggetto con RawImage + VideoPlayer

    private VideoPlayer videoPlayer;

    void Start()
    {
        if (videoBackground == null)
        {
            Debug.LogError("VideoBackground non assegnato in ButtonHoverVideo!");
            return;
        }

        videoPlayer = videoBackground.GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer mancante su VideoBackground!");
        }


        videoBackground.SetActive(false);
    }

    // Quando il mouse entra nel bottone (hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (videoBackground != null)
        {
            videoBackground.SetActive(true);
            if (videoPlayer != null)
                videoPlayer.Play();
        }
    }

    // Quando il mouse esce dal bottone
    public void OnPointerExit(PointerEventData eventData)
    {
        if (videoBackground != null)
        {
            if (videoPlayer != null)
                videoPlayer.Stop();
            videoBackground.SetActive(false);
        }
    }
}

