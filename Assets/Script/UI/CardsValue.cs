using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardsValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }
    public void OnValueChanged(int value)
    {
        txt.text = "Soma " + value;
    }
    public void OnValueTotalChanged(int value)
    {
        txt.text = "Soma Total " + value;
    }
}
