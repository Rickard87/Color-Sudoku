using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize_Main : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }
}
