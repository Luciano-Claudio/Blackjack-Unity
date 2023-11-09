using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    public void OnMoneyChanged(float value)
    {
        txt.text = value.ToString("F");
    }
}
