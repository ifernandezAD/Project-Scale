using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private void Update() { UpdateEscapeGame(); }

    private void UpdateEscapeGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}