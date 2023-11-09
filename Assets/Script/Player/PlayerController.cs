using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [field: SerializeField]
    public float AmoutMoney { get; private set; }

    private void Awake()
    {
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
        Ui.OnMoneyChanged(AmoutMoney);
    }

    public void AddAmountMoney(float amount)
    {
        AmoutMoney += amount;
        Ui.OnMoneyChanged(AmoutMoney);
    }

    public void RemoveAmountMoney(float amount)
    {
        AmoutMoney -= amount;
        Ui.OnMoneyChanged(AmoutMoney);
    }
}
