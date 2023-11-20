using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [field: SerializeField] public Player player { get; private set; }

    private void Awake()
    {
        player = new Player();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private MoneyUi Ui;
    void Start()
    {
        Ui.OnMoneyChanged(player.AmoutMoney);
    }

    public void AddAmountMoney(float amount)
    {
        player.AmoutMoney += amount;
        Ui.OnMoneyChanged(player.AmoutMoney);
    }

    public void RemoveAmountMoney(float amount)
    {
        player.AmoutMoney -= amount;
        Ui.OnMoneyChanged(player.AmoutMoney);
    }
}
