using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private bool paused = false;

    // �� ��ȯ
    public void Restart()
    {
        SceneManager.LoadScene("Settings");
    }

    // �ð��� ���缭 �Ͻ�����
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
