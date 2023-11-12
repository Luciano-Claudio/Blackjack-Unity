using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FimDaPartida : MonoBehaviour
{
    public GameObject WinBanner;
    public GameObject LoseBanner;
    public GameObject DrawBanner;
    public static FimDaPartida instance;
    public TextMeshProUGUI txtWin;
    public TextMeshProUGUI txtDraw;
    private void Awake()
    {
        instance = this;
    }
    public void Win(float bet)
    {
        WinBanner.SetActive(true);
        txtWin.text = "Voc� recebeu " + bet + "R$!";
    }
    public void Lose()
    {
        LoseBanner.SetActive(true);
    }
    public void Draw(float bet)
    {
        DrawBanner.SetActive(true);
        txtDraw.text = "Voc� recebeu de volta seuss " + bet + "R$!";
    }
    public void CloseAll()
    {
        WinBanner.SetActive(false);
        LoseBanner.SetActive(false);
        DrawBanner.SetActive(false);
    }
}
