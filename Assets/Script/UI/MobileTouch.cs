using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MobileTouch : MonoBehaviour
{
    private TMP_InputField input;

    private TouchScreenKeyboard keyboard;

    private void Start()
    {
        input = GetComponent<TMP_InputField>();
    }
    public void OnClick()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
    }
    public void OnClickSecurity()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
    }
    public void OnClickNumber()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad, false, false, false);
    }

    private void Update()
    {
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            input.text = keyboard.text;
            keyboard = null;
        }
    }
}
