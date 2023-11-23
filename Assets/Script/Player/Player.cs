using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    //englider1+blackjackunity@gmail.com
    [field: SerializeField]
    public string Name { get; private set; }
    public string Senha { get; private set; }

    public float AmoutMoney;
    public int Victory1x1;
    public int Victory1x2;
    public int Victory1x3;
    public int Victory2x2;
    public int index;
    public Player()
    {
        AmoutMoney = 10000;
    }
    public Player(string name, string senha, float amoutMoney, int victory1x1, int victory1x2, int victory1x3, int victory2x2)
    {
        Name = name;
        Senha = senha;
        AmoutMoney = amoutMoney;
        Victory1x1 = victory1x1;
        Victory1x2 = victory1x2;
        Victory1x3 = victory1x3;
        Victory2x2 = victory2x2;
    }

    public Player(Row row, int index)
    {
        Name = row.username;
        Senha = row.password;
        AmoutMoney = row.dinheiro;
        Victory1x1 = row.x1;
        Victory1x2 = row.x2;
        Victory1x3 = row.x3;
        Victory2x2 = row.x3;
        this.index = index;
    }

    public Row PlayerToRow()
    {
        return new Row(Name,Senha,AmoutMoney,Victory1x1,Victory1x2,Victory1x3,Victory2x2);
    }
}
