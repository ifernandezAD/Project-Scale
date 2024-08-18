using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance { get; private set; }

    [Header("Cursor Settings")]
    [SerializeField] private bool hideCursorOnStart = true; 
    [SerializeField] private bool enableCursorControl = true; 

    void Awake()
    {
        instance = this;
    }

      private void Start()
    {
        SetCursorVisibility(hideCursorOnStart);
    }

    private void SetCursorVisibility(bool hide)
    {
        Cursor.visible = hide;
        Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
