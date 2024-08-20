using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform spriteRectTransform;  

    [Header("Settings")]
    [SerializeField] private float minSpriteSize = 100f;  
    [SerializeField] private float maxSpriteSize = 400f;  
    [SerializeField] private float maxBlackHoleRadius = 100f;  

    private float currentBlackHoleRadius;

    private void OnEnable()
    {
        BlackHoleBehaviour.onRadiusGrowth += OnRadiusChanged;
    }

    private void Start()
    {
        UpdateSpriteSize();
    }

    private void OnRadiusChanged(float newRadius)
    {
        currentBlackHoleRadius = newRadius;
        UpdateSpriteSize();

        if (currentBlackHoleRadius >= maxBlackHoleRadius)
        {
            CheckVictoryCondition();
        }
    }

    private void UpdateSpriteSize()
    {
        float sizeRatio = Mathf.Clamp01(currentBlackHoleRadius / maxBlackHoleRadius);
        float newSize = Mathf.Lerp(minSpriteSize, maxSpriteSize, sizeRatio);

        spriteRectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    private void CheckVictoryCondition()
    {
        if (Mathf.Approximately(spriteRectTransform.sizeDelta.x, maxSpriteSize))
        {
            Debug.Log("Victory! The sprite has reached the desired size.");
        }
    }

      private void OnDisable()
    {
        BlackHoleBehaviour.onRadiusGrowth -= OnRadiusChanged;
    }
}
