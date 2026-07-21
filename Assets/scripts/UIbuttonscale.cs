using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public float scaleUpFactor = 1.1f;  // How big it grows on hover
    public float animationSpeed = 8f;   // Speed of the animation

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        // Smooth transition between scales
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleUpFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
