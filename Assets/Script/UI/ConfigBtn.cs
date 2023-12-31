using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigBtn : MonoBehaviour
{
    public GameObject MenuPanel;
    [SerializeField] private GameObject FichaPanel;
    public void ConfigClick()
    {
        MenuPanel.SetActive(true);
    }
    public void ConfigClose()
    {
        MenuPanel.SetActive(false);
    }
    public void ClickFicha()
    {
        FichaPanel.SetActive(true);
    }
    public void ExitFicha()
    {
        FichaPanel.SetActive(false);
    }
}
