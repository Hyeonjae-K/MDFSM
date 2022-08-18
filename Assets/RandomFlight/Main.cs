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
        // 메시지 저장을 위한 변수
        string message = "[";
        // 메시지 포맷. 각 위치에 (id, radius, azimuth, elevation)이 담김
        string format = "({0}, {1:F2}, {2:F2}, {3:F2})";
        byte[] datagram;

        // 생성된 드론의 수 만큼 반복문 실행
        for (int i = 0; i < drones.Count; i++)
        {
            string name = drones[i].name;

            // 드론의 좌표를 구 좌표계로 변환
            var pos = drones[i].transform.position;
            float radius = pos.magnitude;
            float azimuth = Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg;
            float elevation = Mathf.Acos(pos.y / radius) * Mathf.Rad2Deg;

            // 메시지에 좌표를 담음
            message += String.Format(format, name, radius, azimuth, elevation);

            if (i + 1 < drones.Count) message += ", ";
        }
        message += "]";

        // 메시지 전송
        datagram = Encoding.UTF8.GetBytes(message);
        udpClient.Send(datagram, datagram.Length, udpHost, udpPort);

        // 디버그 모드일 경우 메시지를 로그에 출력
        if (isDebug) Debug.Log(message);
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();

        // 비행 가능 범위 초기화
        areaCollider = gameObject.GetComponent<BoxCollider>();
        // UDP 통신을 위한 클래스 인스턴스화
        udpClient = new UdpClient();

        // 랜덤비행
        if (!isTrajectory)
        {
            // 입력받은 droneCnt만큼 드론을 생성
            // 드론의 좌표를 읽어 udp로 전송하기 위해
            // 각 드론의 이름을 id로 설정하여 배열에 저장
            for (int i = 0; i < droneCnt; i++)
            {
                GameObject drone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
                drone.name = String.Format("{0:D3}", i);
                drones.Add(drone);
            }
        }
        // 궤적비행
        else
        {
            GameObject drone = Instantiate(trajectoryPrefab, transform.position, Quaternion.identity);
            drone.name = String.Format("{0:D3}", 1);
            drones.Add(drone);
        }

        // 입력받은 udpPeriod를 주기로 드론의 좌표를 전송 (스케줄링)
        if (isSend) InvokeRepeating("SendSphericalCoordinate", 1.0f, udpPeriod);
    }

    // Setting 로드 함수
    void LoadSettings()
    {
        droneCnt = SettingsData.setting_dronecnt;
        trajectoryQuality = SettingsData.setting_trajectoryquality;
        clusterSize = SettingsData.setting_clustersize;
        clusterRange = SettingsData.setting_clusterrange;
        isTrajectory = SettingsData.setting_istrajectory;
        isCluster = SettingsData.setting_iscluster;
        isDebug = SettingsData.setting_isdebug;
        isSend = SettingsData.setting_issend;
        udpHost = SettingsData.setting_udphost;
        udpPort = SettingsData.setting_udpport;
        udpPeriod = SettingsData.setting_udpperiod;

        trajectoryPoints = SettingsData.setting_trajectoryPoints;
    }
}
