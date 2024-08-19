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
            SetupMaterialForTransparency(objectRenderer.material);

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

    private void SetupMaterialForTransparency(Material mat)
    {
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetFloat("_Mode", 2);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}
