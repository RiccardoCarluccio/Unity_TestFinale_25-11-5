using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Animation")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.95f;
    [SerializeField] private float animationSpeed = 10f;
    
    [Header("Color Animation")]
    [SerializeField] private bool animateColor = true;
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 1f);
    
    [Header("Rotation Animation")]
    [SerializeField] private bool rotateOnHover = false;
    [SerializeField] private float rotationAmount = 5f;
    
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Image buttonImage;
    private Color originalColor;
    private bool isHovering = false;
    private bool isPressed = false;
    
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        originalRotation = transform.localRotation;
        targetRotation = originalRotation;
        
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }
    
    void Update()
    {
        // Smooth scale animation
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        
        // Smooth rotation animation
        if (rotateOnHover)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * animationSpeed);
        }
    }
    

    // MOUSE ENTER (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isHovering = true;
            targetScale = originalScale * hoverScale;
            
            if (rotateOnHover)
            {
                targetRotation = Quaternion.Euler(0, 0, rotationAmount);
            }
            
            if (animateColor && buttonImage != null)
            {
                buttonImage.color = hoverColor;
            }
        }
    }
    
    // MOUSE EXIT (Unhover)=
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isHovering = false;
            targetScale = originalScale;
            targetRotation = originalRotation;
            
            if (animateColor && buttonImage != null)
            {
                buttonImage.color = originalColor;
            }
        }
    }
    
    // MOUSE DOWN (Click)
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        targetScale = originalScale * clickScale;
    }
    
    // MOUSE UP (Release)
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        
        if (isHovering)
        {
            targetScale = originalScale * hoverScale;
        }
        else
        {
            targetScale = originalScale;
        }
    }
    

    // PULSE ANIMATION
    public void PulseAnimation()
    {
        StartCoroutine(PulseCoroutine());
    }
    
    private IEnumerator PulseCoroutine()
    {
        Vector3 startScale = transform.localScale;
        Vector3 bigScale = startScale * 1.2f;
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        // Scale up
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, bigScale, t);
            yield return null;
        }
        
        elapsed = 0f;
        
        // Scale down
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(bigScale, startScale, t);
            yield return null;
        }
    }
}
