using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtn : MonoBehaviour
{
    [SerializeField] private GameObject NewGamePanel;
    [SerializeField] private GameObject RankingPanel;
    [SerializeField] private GameObject FichaPanel;
    public void ClickNewGame()
    {
        NewGamePanel.SetActive(true);
    }
    public void ExitNewGame()
    {
        NewGamePanel.SetActive(false);
    }
    public void ClickRanking()
    {
        RankingPanel.SetActive(true);
    }
    public void ExitRanking()
    {
        RankingPanel.SetActive(false);
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
