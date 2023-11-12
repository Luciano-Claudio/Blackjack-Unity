using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    public void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
