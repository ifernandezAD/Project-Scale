using UnityEngine;
using DG.Tweening;

public class AppearWithFade : MonoBehaviour
{
 [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2f; 

    [SerializeField] Renderer objectRenderer;
    private Color originalColor;

    private void Awake()
    {     
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
            SetObjectTransparency(0f);
        }
    }

    private void OnEnable()
    {
        FadeToFullOpacity();
    }

    private void FadeToFullOpacity()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.DOFade(1f, fadeDuration)
                .SetEase(Ease.InOutQuad) 
                .OnComplete(() => Debug.Log("Fade-in complete")); 
        }
    }

    private void SetObjectTransparency(float alpha)
    {
        if (objectRenderer != null)
        {
            Color newColor = originalColor;
            newColor.a = alpha;
            objectRenderer.material.color = newColor;
        }
    }
}
