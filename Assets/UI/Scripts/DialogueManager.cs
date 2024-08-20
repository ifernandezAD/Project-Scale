using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    private int currentLineIndex = 0;

    [SerializeField] private float typingSpeed = 0.05f;

    private bool isTyping = false;

    private void Start()
    {
        if (dialogueLines.Length > 0)
        {
            StartCoroutine(TypeDialogue());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            AdvanceDialogue();
        }
    }

    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void AdvanceDialogue()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueText.text = "";  

        
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Application.Quit();
        }
    }
}

