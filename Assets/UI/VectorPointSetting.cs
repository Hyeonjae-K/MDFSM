using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class VectorPointSetting : MonoBehaviour
{
    // °ªÀ» ÀúÀåÇÒ º¤ÅÍ ÁÂÇ¥
    Vector3 point = new Vector3(0, 0, 0);

    public TMP_InputField xInput;
    public TMP_InputField yInput;
    public TMP_InputField zInput;

    // xÁÂÇ¥ ÀúÀå
    public void SetX()
    {
        point.x = int.Parse(xInput.text);
        SetPoint();
    }

    // yÁÂÇ¥ ÀúÀå
    public void SetY()
    {
        point.y = int.Parse(yInput.text);
        SetPoint();
    }

    // zÁÂÇ¥ ÀúÀå
    public void SetZ()
    {
        point.z = int.Parse(zInput.text);
        SetPoint();
    }

    // º¤ÅÍ ÁÂÇ¥ ¾÷µ¥ÀÌÆ® ÇÔ¼ö
    void SetPoint()
    {
        int num = int.Parse(name.Substring(name.IndexOf(' ') + 1).Trim());
        SettingsData.setting_trajectoryPoints[num] = point;
    }
}
