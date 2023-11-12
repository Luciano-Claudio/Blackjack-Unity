using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    [SerializeField] private GameObject NewGamePanel;
    public void ClickNewGame()
    {
        NewGamePanel.SetActive(true);
    }
}
