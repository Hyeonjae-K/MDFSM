using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField]
    public BoxCollider areaCollider;

    [Header("Drone Count Setting")]
    [SerializeField]
    int droneCnt = 3;
    public GameObject dronePrefab;


    public BoxCollider getAreaCollider()
    {
        return areaCollider;
    }

    // Start is called before the first frame update
    void Start()
    {
        areaCollider = gameObject.GetComponent<BoxCollider>();

        // Instantiate(dronePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        for (int i = 0; i < droneCnt; i++)
        {
            Instantiate(dronePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
