using UnityEngine;

using System.Net.Sockets;
using System.Text;

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

    UdpClient udpClient;


    // 문자열을 전달받아 udp로 전송
    public void SendString(string message)
    {
        byte[] datagram = Encoding.UTF8.GetBytes(message);
        udpClient.Send(datagram, datagram.Length, udpHost, udpPort);

        if (isDebug) Debug.Log(message);
    }


    // Start is called before the first frame update
    void Start()
    {
        areaCollider = gameObject.GetComponent<BoxCollider>();
        udpClient = new UdpClient();

        for (int i = 0; i < droneCnt; i++)
        {
            Instantiate(dronePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
