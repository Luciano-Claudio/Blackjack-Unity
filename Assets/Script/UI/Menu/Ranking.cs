using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Ranking : MonoBehaviour
{
    public GameObject Content;
    public GameObject prefab;
    public List<Row> data;
    private void Start()
    {
        data = APIConnections.instance.data;
    }

    public void ChangeRanking()
    {
        data.Sort();
        foreach (Row r in data)
        {
            Content.SetActive(true);
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
