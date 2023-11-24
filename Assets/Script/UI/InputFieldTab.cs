using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldTab : MonoBehaviour
{
    public Selectable[] UISelectables;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < UISelectables.Length; i++)
            {
                if (UISelectables[i].gameObject == eventSystem.currentSelectedGameObject)
                {
                    UISelectables[(i + 1) % UISelectables.Length].Select();
                    break;
                }
            }
        }
    }
}
