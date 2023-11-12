using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigBtn : MonoBehaviour
{
    public GameObject MenuPanel;
    public void ConfigClick()
    {
        MenuPanel.SetActive(true);
    }
    public void ConfigClose()
    {
        MenuPanel.SetActive(false);
    }
}
