using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingController : MonoBehaviour
{
    public TMP_InputField dronecntInput;
    public TMP_InputField trajectoryqualityInput;
    public TMP_InputField clustersizeInput;
    public TMP_InputField clusterrangeInput;

    public Toggle istrajectoryToggle;
    public Toggle isclusterToggle;

    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeDronecnt()
    {
        SettingsData.setting_dronecnt = int.Parse(dronecntInput.text);
    }

    public void ChangeTrajectoryquality()
    {
        SettingsData.setting_trajectoryquality = int.Parse(trajectoryqualityInput.text);
    }

    public void ChangeClustersize()
    {
        SettingsData.setting_clustersize = int.Parse(clustersizeInput.text);
    }

    public void ChangeClusterrange()
    {
        SettingsData.setting_clusterrange = int.Parse(clusterrangeInput.text);
    }

    public void ToggleTrajectory()
    {
        if (istrajectoryToggle.isOn)
            SettingsData.setting_istrajectory = true;
        else
            SettingsData.setting_istrajectory = false;
    }

    public void ToggleCluster()
    {
        if (isclusterToggle.isOn)
            SettingsData.setting_iscluster = true;
        else
            SettingsData.setting_iscluster = false;
    }
}

public static class SettingsData
{
    public static int setting_dronecnt = 1;
    public static int setting_trajectoryquality = 5;
    public static int setting_clustersize = 0;
    public static int setting_clusterrange = 5;

    public static bool setting_istrajectory = true;
    public static bool setting_iscluster = true;
}
