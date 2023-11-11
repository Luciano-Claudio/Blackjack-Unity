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

    [SerializeField] private CardsValue myCardsValue;
    [SerializeField] private CardsValue aiCardsValue;

    [SerializeField] private float bet;
    [SerializeField] private InputMoney inputMoney;
    [SerializeField] private GameObject btnInicio;
    [SerializeField] private GameObject btnPartida;

    [SerializeField] private bool aiStop;
    [SerializeField] private bool myStop;

    // Start is called before the first frame update
    void Start()
    {
        myValues = 0;
        aiValues = 0;
        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);
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
    public void ButtonStart()
    {
        if (!inputMoney.input.text.Equals(""))
        {            
            float value = float.Parse(inputMoney.input.text);
            PlayerController.instance.RemoveAmountMoney(value);
            bet = value;
            myStop = aiStop = false;
            MyCardsChange();
            AiCardsChange();
            inputMoney.ResetValue();
            btnInicio.SetActive(false);
            btnPartida.SetActive(true);
            ChecarFim();
        }
    }
    public void ButtonContinue()
    {
        MyCardsChange();
        StartCoroutine(AiPlay());
    }

    IEnumerator AiPlay()
    {
        yield return new WaitForSecondsRealtime(.2f);
        AiCardsChange();
        ChecarFim();
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
                if (aux.IsAs)
                {
                    myValues = myValues + 11 > 21 ? myValues + 1 : myValues + 11;
                }
                else
                {
                    myValues += aux.Numero;
                }
                myAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        myCardsValue.OnValueChanged(myValues);

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
                if (aux.IsAs)
                {
                    myValues = myValues + 11 > 21 ? myValues + 1 : myValues + 11;
                }
                else
                {
                    aiValues += aux.Numero;
                }
                aiAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        aiCardsValue.OnValueChanged(aiValues);

        aiQtdCards++;
    }

    private void ChecarFim()
    {
        if((aiValues == 21) || (myValues < aiValues && myStop && aiStop) || (myValues > 21))
        {
            FimDaPartida.instance.Lose();
        }
        else if ((myValues == 21) || (myValues > aiValues && myStop && aiStop) || (aiValues > 21))
        {
            FimDaPartida.instance.Win();
            PlayerController.instance.AddAmountMoney(bet * 2);

        }
        else if((myValues == aiValues && myStop && aiStop))
        {
            FimDaPartida.instance.Draw();
            PlayerController.instance.AddAmountMoney(bet);
        }
    }


}
