using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    public GameObject ResetPromptUI;
    public void ResetPrompt()
    {
        ResetPromptUI.SetActive(true);
    }

    public void ResetAllPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
        ResetPromptUI.SetActive(false);

    }

    public void CloseResetPromptUI()
    {
        ResetPromptUI.SetActive(false);
    }

}
