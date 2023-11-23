using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUi : MonoBehaviour
{
    public TextMeshProUGUI txt;
    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        PlayerController.instance.ValueChanged += OnMoneyChanged;
        OnMoneyChanged();
    }
    public void OnMoneyChanged()
    {
        txt.text = PlayerController.instance.player.AmoutMoney.ToString("F");
    }
}
