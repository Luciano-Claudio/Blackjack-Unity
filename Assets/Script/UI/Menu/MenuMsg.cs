using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuMsg : MonoBehaviour
{

    [SerializeField]
    private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerController.ValueChanged += OnMoneyChanged;
        OnMoneyChanged();
    }
    public void OnMoneyChanged()
    {
        txt.text = "Ol� " + playerController.player.Name + ", voc� tem " + playerController.player.AmoutMoney + " fichas";
    }
}
