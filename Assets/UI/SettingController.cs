using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingController : MonoBehaviour
{
    public TMP_InputField dronecntInput;
    public TMP_InputField trajectoryqualityInput;
    public TMP_InputField trajectorypointsInput;
    public TMP_InputField clustersizeInput;
    public TMP_InputField clusterrangeInput;
    public TMP_InputField udphostInput;
    public TMP_InputField udpportInput;
    public TMP_InputField udpperiodInput;

    public Toggle istrajectoryToggle;
    public Toggle isclusterToggle;
    public Toggle isdebugToggle;
    public Toggle issendToggle;

    public GameObject vectorGroup;
    public GameObject scrollContent;

    // �� �ε� �� ������ �� �ʱ�ȭ
    public void Start()
    {
        SettingsData.setting_dronecnt = 1;
        SettingsData.setting_trajectoryquality = 5;
        SettingsData.setting_trajectorypointcnt = 4;
        SettingsData.setting_clustersize = 0;
        SettingsData.setting_clusterrange = 5;
        SettingsData.setting_udphost = "127.0.0.1";
        SettingsData.setting_udpport = 7777;
        SettingsData.setting_udpperiod = 0.3f;

        SettingsData.setting_istrajectory = false;
        SettingsData.setting_iscluster = false;
        SettingsData.setting_isdebug = false;
        SettingsData.setting_issend = true;
    }

    // ��������� �̵�
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // ���α׷� ���� �Լ�
    public void ExitScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ���� ��� ���� ��ȯ
    public void ChangeDronecnt()
    {
        SettingsData.setting_dronecnt = int.Parse(dronecntInput.text);
    }

    // ���� ���� ����Ƽ ��ȯ
    public void ChangeTrajectoryquality()
    {
        SettingsData.setting_trajectoryquality = int.Parse(trajectoryqualityInput.text);
    }

    // ���� �� ���� ����
    public void ChangeTrajectorypoints()
    {
        SettingsData.setting_trajectorypointcnt = int.Parse(trajectorypointsInput.text);
        SettingsData.setting_trajectoryPoints = new Vector3[SettingsData.setting_trajectorypointcnt];

        for (int i = 0; i < scrollContent.transform.childCount; i++) // �̹� ������ ��ǥ ����
        {
            Destroy(scrollContent.transform.GetChild(i).gameObject);
        }

        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(484, 70 * SettingsData.setting_trajectorypointcnt); // ��ũ�� ũ�� ����

        for (int i = 0; i < SettingsData.setting_trajectorypointcnt; i++) // ��ǥ �Է�ĭ ����
        {
            GameObject Point = Instantiate(vectorGroup, new Vector3(5, -66 * (i + 1), 0), Quaternion.identity);
            Point.transform.SetParent(scrollContent.transform, false);
            Point.name = "Point " + i;
        }
    }

    // ���� ��� ���� ����
    public void ChangeClustersize()
    {
        SettingsData.setting_clustersize = int.Parse(clustersizeInput.text);
    }

    // ���� ���� ����
    public void ChangeClusterrange()
    {
        SettingsData.setting_clusterrange = int.Parse(clusterrangeInput.text);
    }

    // udp ȣ��Ʈ ����
    public void ChangeUdphost()
    {
        SettingsData.setting_udphost = udphostInput.text;
    }

    // udp ��Ʈ ����
    public void ChangeUdpport()
    {
        SettingsData.setting_udpport = int.Parse(udpportInput.text);
    }

    // udp period ����
    public void ChangeUdpperiod()
    {
        SettingsData.setting_udpperiod = float.Parse(clusterrangeInput.text);
    }

    // ���� / ���� ����
    public void ToggleTrajectory()
    {
        if (istrajectoryToggle.isOn)
            SettingsData.setting_istrajectory = true;
        else
            SettingsData.setting_istrajectory = false;
    }

    // ���� �¿���
    public void ToggleCluster()
    {
        if (isclusterToggle.isOn)
            SettingsData.setting_iscluster = true;
        else
            SettingsData.setting_iscluster = false;
    }

    // ����� �¿���
    public void ToggleDebug()
    {
        if (isdebugToggle.isOn)
            SettingsData.setting_isdebug = true;
        else
            SettingsData.setting_isdebug = false;
    }

    // �� ���� �¿���
    public void ToggleSend()
    {
        if (issendToggle.isOn)
            SettingsData.setting_issend = true;
        else
            SettingsData.setting_issend = false;
    }
}

// ���� �� ���� Ŭ����
public static class SettingsData
{
    public static int setting_dronecnt = 1;
    public static int setting_trajectoryquality = 5;
    public static int setting_trajectorypointcnt = 4;
    public static int setting_clustersize = 0;
    public static int setting_clusterrange = 5;
    public static string setting_udphost = "127.0.0.1";
    public static int setting_udpport = 7777;
    public static float setting_udpperiod = 0.3f;

    public static bool setting_istrajectory = false;
    public static bool setting_iscluster = false;
    public static bool setting_isdebug = false;
    public static bool setting_issend = true;

    public static Vector3[] setting_trajectoryPoints;
}
