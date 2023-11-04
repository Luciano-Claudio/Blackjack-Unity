using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Cards", menuName = "Cards/New Card")]
public class Cards : ScriptableObject
{
    public Naipe MyProperty;
    public int Numero;
    public bool IsAs;
    public Sprite Image;

}
