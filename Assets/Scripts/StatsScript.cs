using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{
    public GameObject statsUI;
    public Text statsText;

    public void OpenStatsUI()
    {
        statsUI.SetActive(true);
    }
    public void CloseStatsUI()
    {
        statsUI.SetActive(false);
    }
    public void SetStatsText()
    {
        statsText.text =
            "Easy: " + PlayerPrefs.GetInt("easyClear").ToString() + "\n" +
            "Medium: " + PlayerPrefs.GetInt("mediumClear").ToString() + "\n" +
            "Hard: " + PlayerPrefs.GetInt("hardClear").ToString() + "\n" +
            "Hardest: " + PlayerPrefs.GetInt("nightmareClear").ToString() + "\n";
    }
}
