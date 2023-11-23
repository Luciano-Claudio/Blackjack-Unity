using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    [SerializeField] private GameObject CreateAccountPanel;
    [SerializeField] private GameObject AccountCreatedPanel;

    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI erro;


    private void Start()
    {
        erro.enabled = false;
    }

    public void ConnectAccount()
    {
        try
        {
            APIConnections.instance.ConnectPlayer(username.text, password.text);
            erro.enabled = false;
            StartCoroutine(LoadYourAsyncScene("Menu"));
        }
        catch (AccountException e)
        {
            erro.enabled = true;
            erro.text = e.Message;
        }
    }
    IEnumerator LoadYourAsyncScene(string level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OpenCreateAccount()
    {
        CreateAccountPanel.SetActive(true);
        erro.enabled = false;
    }

    public void CloseCreateAccount()
    {
        CreateAccountPanel.SetActive(false);
        AccountCreatedPanel.SetActive(false);
    }
}
