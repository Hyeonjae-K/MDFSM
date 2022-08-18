using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class VectorPointSetting : MonoBehaviour
{
    // ���� ������ ���� ��ǥ
    Vector3 point = new Vector3(0, 0, 0);

    public TMP_InputField xInput;
    public TMP_InputField yInput;
    public TMP_InputField zInput;

    // x��ǥ ����
    public void SetX()
    {
        point.x = int.Parse(xInput.text);
        SetPoint();
    }

    // y��ǥ ����
    public void SetY()
    {
        point.y = int.Parse(yInput.text);
        SetPoint();
    }

    // z��ǥ ����
    public void SetZ()
    {
        point.z = int.Parse(zInput.text);
        SetPoint();
    }

    // ���� ��ǥ ������Ʈ �Լ�
    void SetPoint()
    {
        int num = int.Parse(name.Substring(name.IndexOf(' ') + 1).Trim());
        SettingsData.setting_trajectoryPoints[num] = point;
    }
}
