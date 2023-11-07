using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SomaCartas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private GameController game;
    void Start()
    {
        game.ChangeMyValue += OnMyValueChanged;
    }


    private void OnMyValueChanged(int value)
    {
        txt.text = "Soma " + value;
    }
}
