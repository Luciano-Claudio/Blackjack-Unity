using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FimDaPartida : MonoBehaviour
{
    public GameObject WinBanner;
    public GameObject LoseBanner;
    public GameObject DrawBanner;
    public static FimDaPartida instance;
    private void Awake()
    {
        instance = this;
    }
    public void Win()
    {
        WinBanner.SetActive(true);
    }
    public void Lose()
    {
        LoseBanner.SetActive(true);
    }
    public void Draw()
    {
        DrawBanner.SetActive(true);
    }
    public void CloseAll()
    {
        WinBanner.SetActive(false);
        LoseBanner.SetActive(false);
        DrawBanner.SetActive(false);
    }
}
