using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [field: SerializeField] public Player player { get; private set; }

    public delegate void ValueHandler();
    public event ValueHandler ValueChanged;

    private void Awake()
    {
        if (instance == null) instance = this;
        player = APIConnections.instance.player;
    }
    public void AddAmountMoney(float amount, int type)
    {
        player.AmoutMoney += amount;
        if (type != 0) AddVictory(type);
        APIConnections.instance.SendAccountDynamoDB(player.PlayerToRow());
        if (ValueChanged != null)
            ValueChanged();
    }

    public void RemoveAmountMoney(float amount)
    {
        player.AmoutMoney -= amount;
        APIConnections.instance.SendAccountDynamoDB(player.PlayerToRow());
        if (ValueChanged != null)
            ValueChanged();
    }

    public void AddVictory(int type)
    {
        switch (type)
        {
            case 1:
                player.Victory1x1++;
                break;
            case 2:
                player.Victory1x2++;
                break;
            case 3:
                player.Victory1x3++;
                break;
            case 4:
                player.Victory2x2++;
                break;
        }
    }
        
}
