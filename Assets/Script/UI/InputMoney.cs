using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InputMoney : MonoBehaviour
{
    public TMP_InputField input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ValueChange()
    {
        string str = input.text;
        if (!str.Equals(""))
        {
            str = GetNumbers(str);
            if (float.Parse(str) > PlayerController.instance.player.AmoutMoney)
            {
                str = PlayerController.instance.player.AmoutMoney.ToString();
            }
            input.text = str;
        }
    }

    public void ResetValue()
    {
        input.text = "";
    }

    public string GetNumbers(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }
}