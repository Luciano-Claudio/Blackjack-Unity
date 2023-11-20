using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    [field: SerializeField]
    public string Name { get; private set; }
    public string Senha { get; private set; }

    public float AmoutMoney;
    public int Victory1x1;
    public int Victory1x2;
    public int Victory1x3;
    public int Victory2x2;

    public Player()
    {
        AmoutMoney = 1000;
    }
}
