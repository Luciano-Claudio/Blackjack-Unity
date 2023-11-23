using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class TestSendBD : MonoBehaviour
{
    public TMP_InputField user, password;


    public void SendAccountDynamoDB()
    {
        StartCoroutine(SendEnumerator());
    }
    IEnumerator SendEnumerator()
    {
        yield return new WaitForSecondsRealtime(1);
        UnityWebRequest www = new UnityWebRequest("https://lg6vzckjf9.execute-api.us-east-2.amazonaws.com/Blackjack/user");
        string aux = "{\"username\":\"" + user.text + "\", \"password\":\"" + password.text + "\"}";
        Debug.Log(aux);
        www.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(aux));
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
