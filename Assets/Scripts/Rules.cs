using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    public GameObject rulesPanel;
    public void OpenRulesPanel()
    {
        rulesPanel.SetActive(true);
    }
    public void CloseRulesPanel()
    {
        rulesPanel.SetActive(false);
    }
}
