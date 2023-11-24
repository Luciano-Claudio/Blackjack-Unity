using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateAccount : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI erro;
    [SerializeField] private GameObject succesPanel;
    [SerializeField] private GameObject CreateAccountPanel;

    private void Start()
    {
        erro.enabled = false;
    }

    public void CreateAccountSend()
    {
        try
        {
            CheckData();
            APIConnections.instance.SendAccountDynamoDB(username.text, password.text);
            erro.enabled = false;
            succesPanel.SetActive(true);
        }
        catch (AccountException e)
        {
            erro.enabled = true;
            erro.text = e.Message;
        }
    }

    private void CheckData()
    {
        if (username.text.Equals("")) throw new AccountException("o usuário não pode ser vazio!");
        if (password.text.Equals("")) throw new AccountException("a senha não pode ser vazia!");
        if (username.text.Contains(" ")) throw new AccountException("o usuário não pode conter espaços!");
        if (password.text.Contains(" ")) throw new AccountException("a senha não pode conter espaços!");
        if (username.text.Length < 6) throw new AccountException("o usuário deve conter pelo menos 6 digitos!");
        if (password.text.Length < 6) throw new AccountException("a senha deve conter pelo menos 6 digitos!");
    }
    public void CloseCreateAccount()
    {
        username.text = "";
        password.text = "";
        gameObject.SetActive(false);
    }
}
