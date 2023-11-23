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

    public void AddAmountMoney(float amount)
    {
        player.AmoutMoney += amount;
        APIConnections.instance.data[player.index] = player.PlayerToRow();
        APIConnections.instance.SendAccountDynamoDB(player.PlayerToRow());
        if (ValueChanged != null)
            ValueChanged();
    }

    public void RemoveAmountMoney(float amount)
    {
        player.AmoutMoney -= amount;
        APIConnections.instance.data[player.index] = player.PlayerToRow();
        APIConnections.instance.SendAccountDynamoDB(player.PlayerToRow());
        if (ValueChanged != null)
            ValueChanged();
    }
}
