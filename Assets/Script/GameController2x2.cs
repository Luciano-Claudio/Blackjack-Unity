using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class GameController2x2 : MonoBehaviour
{
    [SerializeField] private List<Cards> cards;
    [SerializeField] private Stack<Cards> cardsStack;

    [SerializeField] private GameObject myLocalCards;
    [SerializeField] private GameObject aiLocalCards;
    [SerializeField] private GameObject alliedAILocalCards; // Nova variável para a IA aliada
    [SerializeField] private GameObject enemyAILocalCards;  // Nova variável para a IA adversária

    public int myValues { get; private set; }
    public int aiValues { get; private set; }
    public int alliedaiValues { get; private set; }
    public int enemyaiValues { get; private set; }

    [SerializeField] private int myQtdCards;
    [SerializeField] private int aiQtdCards;
    [SerializeField] private int alliedAIQtdCards;
    [SerializeField] private int enemyAIQtdCards;

    [SerializeField] private Transform myAtualSpot;
    [SerializeField] private Transform aiAtualSpot;
    [SerializeField] private Transform alliedAIAtualSpot;
    [SerializeField] private Transform enemyAIAtualSpot;

    [SerializeField] private CardsValue myCardsValue;
    [SerializeField] private CardsValue aiCardsValue;
    [SerializeField] private CardsValue alliedAICardsValue;
    [SerializeField] private CardsValue enemyAICardsValue;

    [SerializeField] private float bet;
    [SerializeField] private InputMoney inputMoney;
    [SerializeField] private GameObject btnInicio;
    [SerializeField] private GameObject btnPartida;

    [SerializeField] private bool aiStop;
    [SerializeField] private bool myStop;

    void Start()
    {
        myQtdCards = 2;
        aiQtdCards = 2;
        alliedAIQtdCards = 2;
        enemyAIQtdCards = 2;
        myAtualSpot = null;
        aiAtualSpot = null;
        alliedAIAtualSpot = null;
        enemyAIAtualSpot = null;
        myValues = 0;
        aiValues = 0;
        alliedaiValues = 0;
        enemyaiValues = 0;

        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);
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
            AlliedAICardsChange();  // Novo método para a IA aliada
            EnemyAICardsChange();   // Novo método para a IA adversária
            inputMoney.ResetValue();
            btnInicio.SetActive(false);
            btnPartida.SetActive(true);
            ChecarFim();
        }
    }

    public void ButtonContinue()
    {
        MyCardsChange();
        ChecarFim();
        StartCoroutine(AiPlay());
        AlliedAICardsChange();  // Novo método para a IA aliada
        EnemyAICardsChange();   // Novo método para a IA adversária
    }

    IEnumerator AiPlay()
    {
        yield return new WaitForSecondsRealtime(.5f);
        AiCardsChange();
        ChecarFim();
        yield return new WaitForSecondsRealtime(.5f);
        AlliedAICardsChange();  // Novo método para a IA aliada
        ChecarFim();
        yield return new WaitForSecondsRealtime(.5f);
        EnemyAICardsChange();   // Novo método para a IA adversária
        ChecarFim();
    }

    public void MyCardsChange()
    {
        Transform auxSpot = null;
        if (myAtualSpot != null)
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

        for (int i = 0; i < aiQtdCards; i++)
        {
            if (auxSpot != null && i < aiQtdCards - 1)
            {
                aiAtualSpot.GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();
                if (aux.IsAs)
                {
                    aiValues = aiValues + 11 > 21 ? aiValues + 1 : aiValues + 11;
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

    public void AlliedAICardsChange()  // Novo método para a IA aliada
    {
        Transform auxSpot = null;
        if (alliedAIAtualSpot != null)
        {
            auxSpot = alliedAIAtualSpot;
            auxSpot.gameObject.SetActive(false);
        }
        alliedAIAtualSpot = alliedAILocalCards.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.gameObject.CompareTag(alliedAIQtdCards + " Cards"));

        alliedAIAtualSpot.gameObject.SetActive(true);

        for (int i = 0; i < alliedAIQtdCards; i++)
        {
            if (auxSpot != null && i < alliedAIQtdCards - 1)
            {
                alliedAIAtualSpot.GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();
                if (aux.IsAs)
                {
                    alliedaiValues = alliedaiValues + 11 > 21 ? alliedaiValues + 1 : alliedaiValues + 11;
                }
                else
                {
                    alliedaiValues += aux.Numero;
                }
                alliedAIAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        alliedAICardsValue.OnValueChanged(alliedaiValues);

        alliedAIQtdCards++;
    }

    public void EnemyAICardsChange()  // Novo método para a IA adversária
    {
        Transform auxSpot = null;
        if (enemyAIAtualSpot != null)
        {
            auxSpot = enemyAIAtualSpot;
            auxSpot.gameObject.SetActive(false);
        }
        enemyAIAtualSpot = enemyAILocalCards.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.gameObject.CompareTag(enemyAIQtdCards + " Cards"));

        enemyAIAtualSpot.gameObject.SetActive(true);

        for (int i = 0; i < enemyAIQtdCards; i++)
        {
            if (auxSpot != null && i < enemyAIQtdCards - 1)
            {
                enemyAIAtualSpot.GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();
                if (aux.IsAs)
                {
                    enemyaiValues = enemyaiValues + 11 > 21 ? enemyaiValues + 1 : enemyaiValues + 11;
                }
                else
                {
                    enemyaiValues += aux.Numero;
                }
                enemyAIAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        enemyAICardsValue.OnValueChanged(enemyaiValues);

        enemyAIQtdCards++;
    }
    private void CheckAiStop()
    {
        if (aiStop) return;
        if (myValues > aiValues && myValues < 21) return;
        if (myValues < aiValues && myStop) aiStop = true;
        else if (aiValues == 20) aiStop = true;
        else if (aiValues == 19)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) aiStop = true;
        }
        else if (aiValues == 18)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 95) aiStop = true;
        }
        else if (aiValues == 17)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 90) aiStop = true;
        }
    }

    private void ChecarFim()
    {
        if ((aiValues == 21) || (myValues < aiValues && myStop && aiStop) || (myValues > 21))
        {
            //FimDaPartida.instance.Lose();
        }
        else if ((myValues == 21) || (myValues > aiValues && myStop && aiStop) || (aiValues > 21))
        {
            //FimDaPartida.instance.Win();
            //PlayerController.instance.AddAmountMoney(bet * 2);
        }
        else if ((myValues == aiValues && myStop && aiStop))
        {
            //FimDaPartida.instance.Draw();
            //PlayerController.instance.AddAmountMoney(bet);
        }

        // Checando também para a IA aliada e adversária
        if ((alliedaiValues == 21) || (myValues < alliedaiValues && myStop) || (myValues > 21))
        {
            // Lógica para a IA aliada
        }
        else if ((myValues == 21) || (myValues > alliedaiValues && myStop) || (alliedaiValues > 21))
        {
            // Lógica para a IA aliada
        }

        if ((enemyaiValues == 21) || (myValues < enemyaiValues && myStop) || (myValues > 21))
        {
            // Lógica para a IA adversária
        }
        else if ((myValues == 21) || (myValues > enemyaiValues && myStop) || (enemyaiValues > 21))
        {
            // Lógica para a IA adversária
        }
    }
}
