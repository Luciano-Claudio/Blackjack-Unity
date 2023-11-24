using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnections : MonoBehaviour
{
    [System.Serializable]
    public class Wrapper
    {
        public List<Row> list;
    }
    public static APIConnections instance;
    [field: SerializeField] public List<Row> data { get; private set; }
    [field: SerializeField] public Player player { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        data = new List<Row>();
        StartCoroutine(GetText());
    }
    IEnumerator GetText()
    {
        UnityWebRequest www = new UnityWebRequest("https://lg6vzckjf9.execute-api.us-east-2.amazonaws.com/Blackjack/ranking");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Wrapper w = JsonUtility.FromJson<Wrapper>(www.downloadHandler.text);
            data = w.list;
        }
        foreach(Row r in data)
        {
            Debug.Log(r.username + " " + r.password);
        }
    }

    public void SendAccountDynamoDB(string username, string password)
    {
        if (FindAccount(username)) throw new AccountException("nome de usuário já existe!");
        StartCoroutine(SendEnumerator(username,password));
    }

    IEnumerator SendEnumerator(string username, string password)
    {
        yield return new WaitForSecondsRealtime(1);
        string url = "https://lg6vzckjf9.execute-api.us-east-2.amazonaws.com/Blackjack/user";
        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
        string json = "{\"username\":\"" + username 
            + "\", \"password\":\"" + password+ "\""
            + "}";
        Debug.Log(json);
        Row r = new Row(username, password);
        data.Add(r);
        www.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(json));
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
    public void SendAccountDynamoDB(Row row)
    {
        StartCoroutine(SendEnumerator(row));
    }

    IEnumerator SendEnumerator(Row row)
    {
        yield return new WaitForSecondsRealtime(1);
        string url = "https://lg6vzckjf9.execute-api.us-east-2.amazonaws.com/Blackjack/user";
        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
        string json = "{\"username\":\"" + row.username
            + "\", \"password\":\"" + row.password 
            + "\", \"dinheiro\":\"" + row.dinheiro 
            + "\", \"x1\":\"" + row.x1 
            + "\", \"x2\":\"" + row.x2 
            + "\", \"x3\":\"" + row.x3 
            + "\", \"x22\":\"" + row.x22
            + "\""
            + "}";
        Debug.Log(json);
        data.RemoveAt(player.index);
        data.Add(row);
        player.index = data.IndexOf(row);
        www.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(json));
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
    public bool FindAccount(string username)
    {
        if (data.Find(x => x.username == username) == null) return false;
        return true;
    }
    public void ConnectPlayer(string username, string password)
    {
        Row row = data.Find(x => x.username == username);
        if (row == null) throw new AccountException("nome de usuário não existe!");
        if(!row.password.Equals(password)) throw new AccountException("senha incorreta!");
        
        player = new Player(row, data.IndexOf(row));
        Debug.Log(data.IndexOf(row));
    }
}
