using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteRanking : MonoBehaviour
{
    public GameObject Content;
    public List<GameObject> RankingPrefabs;

    private void Start()
    {
        foreach(GameObject prefab in RankingPrefabs)
        {
            Instantiate(prefab, Content.transform);
        }
    }
}
