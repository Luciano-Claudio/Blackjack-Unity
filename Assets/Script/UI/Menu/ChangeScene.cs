using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneStr(string level)
    {
        Debug.Log(level);
        StartCoroutine(LoadYourAsyncScene(level));
    }
    IEnumerator LoadYourAsyncScene(string level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
