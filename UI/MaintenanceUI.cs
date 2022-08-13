using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceUI : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
