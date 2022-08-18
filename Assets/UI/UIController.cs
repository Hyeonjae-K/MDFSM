using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private bool paused = false;

    // æ¿ ¿¸»Ø
    public void Restart()
    {
        SceneManager.LoadScene("Settings");
    }

    // Ω√∞£¿ª ∏ÿ√Áº≠ ¿œΩ√¡§¡ˆ
    public void Pause()
    {
        if (paused)
        {
            Debug.Log("Resumed");
            Time.timeScale = 1f;
            paused = false;
        }
        else
        {
            Debug.Log("Paused");
            Time.timeScale = 0;
            paused = true;
        }
    }
}
