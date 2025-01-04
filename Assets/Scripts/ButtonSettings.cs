using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSettings : MonoBehaviour
{
    public void ButtonClick(string setting)
    {
        if (setting == "easy")
        {
            Settings.difficulty = Settings.Difficulties.EASY;
        }
        if (setting == "medium")
        {
            Settings.difficulty = Settings.Difficulties.MEDIUM;
        }
        if (setting == "hard")
        {
            Settings.difficulty = Settings.Difficulties.HARD;
        }
        if (setting == "nightmare")
        {
            Settings.difficulty = Settings.Difficulties.NIGHTMARE;
        }

        if (PlayerPrefs.GetInt("loadCount") > 2)
        {
            GameObject.Find("AdManager").GetComponent<AdManager>().ShowInterstitialAd();
        }
        else
        {
            PlayerPrefs.SetInt("loadCount", PlayerPrefs.GetInt("loadCount") + 1);
            SceneManager.LoadScene("Game");
        }

    }

    public void Replay()
    {
        if (PlayerPrefs.GetInt("loadCount") > 2)
        {
            GameObject.Find("AdManager").GetComponent<AdManager>().ShowInterstitialAd();
        }
        else
        {
            PlayerPrefs.SetInt("loadCount", PlayerPrefs.GetInt("loadCount") + 1);
            SceneManager.LoadScene("Game");
        }
    }
    public void MainMenu()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    public void Return()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
