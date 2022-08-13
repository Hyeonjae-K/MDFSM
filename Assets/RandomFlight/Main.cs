using UnityEngine;

using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System;

public class Main : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField]
    public BoxCollider areaCollider;
     

    [Header("Drone Count Setting")]
    [SerializeField]
    int droneCnt = 3;
    public GameObject dronePrefab;

    [Header("System Settings")]
    [SerializeField]
    public bool isDebug = true;
    [SerializeField]
    public bool isSend = false;
    [SerializeField]
    string udpHost = "127.0.0.1";
    [SerializeField]
    int udpPort = 7777;
    [SerializeField]
    public float udpPeriod = 0.3f;

    [Header("Trajectory Flight Settings")]
    [SerializeField]
    public bool isTrajectory = false;
    [SerializeField]
    protected spline spline;
    public GameObject trajectoryPrefab;
    public int trajectoryQuality = 10;
    public Vector3[] trajectoryPoints;

    [Header("Cluster Flight Settings")]
    [SerializeField]
    public bool isCluster = false;
    [SerializeField]
    public int clusterSize = 2;
    public GameObject clusterDronePrefab;
    [SerializeField]
    public float clusterRange = 5f;

    UdpClient udpClient;

    List<GameObject> drones = new List<GameObject>();

    // 3차원 벡터를 구 좌표로 변환하여 udp로 전송
    void SendSphericalCoordinate()
    {
        string message = "[";
        // id, radius, azimuth, elevation
        string format = "({0}, {1:F2}, {2:F2}, {3:F2})";
        byte[] datagram;

        for (int i = 0; i < drones.Count; i++)
        {
            string name = drones[i].name;

            var pos = drones[i].transform.position;
            float radius = pos.magnitude;
            float azimuth = Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg;
            float elevation = Mathf.Acos(pos.y / radius) * Mathf.Rad2Deg;

            Debug.Log(String.Format(format, name, radius, azimuth, elevation));
            message += String.Format(format, name, radius, azimuth, elevation);

            if (i + 1 < drones.Count) message += ", ";
        }
        message += "]";

        datagram = Encoding.UTF8.GetBytes(message);
        udpClient.Send(datagram, datagram.Length, udpHost, udpPort);

        if (isDebug) Debug.Log(message);
    }


    // Start is called before the first frame update
    void Start()
    {
        areaCollider = gameObject.GetComponent<BoxCollider>();
        udpClient = new UdpClient();

        if (!isTrajectory)
        {
            for (int i = 0; i < droneCnt; i++)
            {
                GameObject drone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
                drone.name = String.Format("{0:D3}", i);
                drones.Add(drone);
            }
        }
        else
        {
            GameObject drone = Instantiate(trajectoryPrefab, transform.position, Quaternion.identity);
            drone.name = String.Format("{0:D3}", 1);
            drones.Add(drone);
        }

        if (isSend) InvokeRepeating("SendSphericalCoordinate", 1.0f, udpPeriod);
    }
}
