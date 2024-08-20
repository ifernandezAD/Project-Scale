using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform spriteRectTransform;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Settings")]
    [SerializeField] private float minSpriteSize = 100f;
    [SerializeField] private float maxSpriteSize = 400f;
    [SerializeField] private float maxBlackHoleRadius = 100f;
    [SerializeField] private float countdownTime = 180f;  

    private float currentBlackHoleRadius;
    private float timeRemaining;

    private void OnEnable()
    {
        BlackHoleBehaviour.onRadiusGrowth += OnRadiusChanged;
        StartCountdown();
    }

    private void Start()
    {
        UpdateSpriteSize();
        UpdateTimerDisplay();
    }

    private void OnRadiusChanged(float newRadius)
    {
        currentBlackHoleRadius = newRadius;
        UpdateSpriteSize();
    }

    private void UpdateSpriteSize()
    {
        float sizeRatio = Mathf.Clamp01(currentBlackHoleRadius / maxBlackHoleRadius);
        float newSize = Mathf.Lerp(minSpriteSize, maxSpriteSize, sizeRatio);

        spriteRectTransform.sizeDelta = new Vector2(newSize, newSize);
    }

    private void StartCountdown()
    {
        timeRemaining = countdownTime;
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        OnCountdownFinished();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnCountdownFinished()
    {
        if (currentBlackHoleRadius >= maxBlackHoleRadius)
        {
            Debug.Log("Victory! The sprite has reached the desired size.");
            SceneManager.LoadScene("Victory");
        }
        else
        {
            Debug.Log("Time's up! You failed to complete the objective.");
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnDisable()
    {
        BlackHoleBehaviour.onRadiusGrowth -= OnRadiusChanged;
    }
}
