using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Settings");
    }

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
