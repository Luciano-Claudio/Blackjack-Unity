using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class GameController : MonoBehaviour
{
    [SerializeField] private List<Cards> cards;
    [SerializeField] private Stack<Cards> cardsStack;

    [SerializeField] private GameObject myLocalCards;
    [SerializeField] private GameObject aiLocalCards;
    public int myValues { get; private set; }
    public int aiValues { get; private set; }

    [SerializeField] private int myQtdCards = 2;
    [SerializeField] private int aiQtdCards = 2;

    [SerializeField] private Transform myAtualSpot = null;
    [SerializeField] private Transform aiAtualSpot = null;

    public delegate void ChangeMyValueHandler(int value);
    public event ChangeMyValueHandler ChangeMyValue;

    // Start is called before the first frame update
    void Start()
    {
        myValues = 0;
        aiValues = 0;
        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);

        MyCardsChange();
        AiCardsChange();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Shuffle<T>(List<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;

        for (int i = 0; i < n; i++)
        {
            int randomIndex = i + rng.Next(n - i);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void MyCardsChange()
    {
        Transform auxSpot = null;
        if(myAtualSpot != null)
        {
            auxSpot = myAtualSpot;
            auxSpot.gameObject.SetActive(false);
        }
        myAtualSpot = myLocalCards.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.gameObject.CompareTag(myQtdCards + " Cards"));

        myAtualSpot.gameObject.SetActive(true);

        for (int i = 0; i < myQtdCards; i++)
        {
            if (auxSpot != null && i < myQtdCards - 1)
            {
                myAtualSpot.GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();

                myValues += aux.Numero;
                myAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        if(ChangeMyValue != null)
            ChangeMyValue(myValues);

        myQtdCards++;
    }
    public void AiCardsChange()
    {
        Transform auxSpot = null;
        if (aiAtualSpot != null)
        {
            auxSpot = aiAtualSpot;
            auxSpot.gameObject.SetActive(false);
        }
        aiAtualSpot = aiLocalCards.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.gameObject.CompareTag(aiQtdCards + " Cards"));

        aiAtualSpot.gameObject.SetActive(true);

        for(int i = 0; i < aiQtdCards; i++)
        {
            if (auxSpot != null && i < aiQtdCards -1) 
            {
                aiAtualSpot.GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();

                aiValues += aux.Numero;
                aiAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }

        aiQtdCards++;
    }


}
