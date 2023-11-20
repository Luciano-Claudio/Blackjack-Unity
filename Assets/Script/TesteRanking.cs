using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class TesteRanking : MonoBehaviour
{
    [System.Serializable]
    public class Row
    {
        public string username;
        public int dinheiro;
        public int x1;
        public int x2;
        public int x3;
        public int x22;
    }
    [System.Serializable]
    public class Wrapper
    {
        public List<Row> list;
    }
    public GameObject Content;
    public GameObject prefab;

    private void Start()
    {
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
            foreach (Row r in w.list)
            {
                GameObject c = Instantiate(prefab, Content.transform);
                Transform panel = c.transform.Find("Panel");
                panel.Find("Nome").GetComponent<TextMeshProUGUI>().text = r.username;
                panel.Find("1x1").GetComponent<TextMeshProUGUI>().text = r.x1.ToString();
                panel.Find("1x2").GetComponent<TextMeshProUGUI>().text = r.x2.ToString();
                panel.Find("1x3").GetComponent<TextMeshProUGUI>().text = r.x3.ToString();
                panel.Find("2x2").GetComponent<TextMeshProUGUI>().text = r.x22.ToString();
            }
        }
    }
}
