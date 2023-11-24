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

    [SerializeField] private GameObject LoadingGif;
    [SerializeField] private GameObject myLocalCards;
    [SerializeField] private GameObject aiLocalCards;
    [SerializeField] private GameObject alliedAILocalCards;
    [SerializeField] private GameObject enemyAILocalCards;

    public int sumAllyValues { get; set; }
    public int sumAiValues { get; set; }
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
    [SerializeField] private CardsValue totalEnemyAICardsValue;
    [SerializeField] private CardsValue totalAllyCardsValue;

    [SerializeField] private float bet;
    [SerializeField] private InputMoney inputMoney;
    [SerializeField] private GameObject btnInicio;
    [SerializeField] private GameObject btnPartida;
    [SerializeField] private GameObject btnNovaPartida;

    [SerializeField] private bool myStop;
    [SerializeField] private bool aiStop;
    [SerializeField] private bool enemyAIStop;
    [SerializeField] private bool alliedAIStop;

    [SerializeField] private bool end;
    [SerializeField] private Button continuarBtn;

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
            bet = value;
            FimDaPartida.instance.FakeRemove(bet);
            myStop = aiStop = false;
            MyCardsChange();
            AiCardsChange();
            AlliedAICardsChange();
            EnemyAICardsChange();
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
    }

    public void ButtonStop()
    {
        if (myStop) return;
        myStop = true;
        continuarBtn.interactable = false;
        StartCoroutine(AiPlay());
    }

    IEnumerator AiPlay()
    {
        btnPartida.SetActive(false);
        LoadingGif.SetActive(true);
        if (!end)
        {
            yield return new WaitForSecondsRealtime(1f);
            AiCardsChange();
            ChecarFim();
        }
        if (!end)
        {
            yield return new WaitForSecondsRealtime(1f);
            AlliedAICardsChange();
            ChecarFim();
        }
        if (!end)
        {
            yield return new WaitForSecondsRealtime(1f);
            EnemyAICardsChange();
            ChecarFim();
        }
        if (!myStop && !end)
        {
            btnPartida.SetActive(true);

            LoadingGif.SetActive(false);
        }

        if (!end && myStop) StartCoroutine(AiPlay());
    }

    public void MyCardsChange()
    {
        if (myStop) return;
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
        sumAllyValues = myValues + alliedaiValues;
        totalAllyCardsValue.OnValueChanged(sumAllyValues);

        if (myValues == 21) ButtonStop();

        myQtdCards++;
    }

    public void AiCardsChange()
    {
        CheckAiStop();

        if (aiStop) return;

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
        sumAiValues = enemyaiValues + aiValues;
        totalEnemyAICardsValue.OnValueChanged(sumAiValues);

        aiQtdCards++;
    }

    public void AlliedAICardsChange()
    {
        CheckalliedStop();

        if (alliedAIStop) return;
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
        sumAllyValues = myValues + alliedaiValues;
        totalAllyCardsValue.OnValueChanged(sumAllyValues);

        alliedAIQtdCards++;
    }

    public void EnemyAICardsChange()
    {
        CheckEnemyAIStop();

        if (enemyAIStop) return;
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
        sumAiValues = enemyaiValues + aiValues;
        totalEnemyAICardsValue.OnValueChanged(sumAiValues);

        enemyAIQtdCards++;
    }
    private void CheckalliedStop()
    {
        if (alliedAIStop) return;
        if (sumAiValues > sumAllyValues && sumAiValues < 42) return;
        if (sumAllyValues > sumAiValues && sumAllyValues >= 37) alliedAIStop = true;
        if (sumAiValues <= sumAllyValues && aiStop && enemyAIStop) alliedAIStop = true;
        if (alliedaiValues >= 20) alliedAIStop = true;
        else if (alliedaiValues == 19)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 99) alliedAIStop = true;
        }
        else if (alliedaiValues == 18)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) alliedAIStop = true;
        }
        else if (alliedaiValues == 17)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 90) alliedAIStop = true;
        }
    }
    private void CheckAiStop()
    {
        if (aiStop) return;
        if (sumAllyValues > sumAiValues && sumAllyValues < 42) return;
        if (sumAiValues > sumAllyValues && sumAiValues >= 37) aiStop = true;
        if (sumAllyValues <= sumAiValues && myStop && alliedAIStop) aiStop = true;
        if (aiValues >= 20) aiStop = true;
        else if (aiValues == 19)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 99) aiStop = true;
        }
        else if (aiValues == 18)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) aiStop = true;
        }
        else if (aiValues == 17)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 90) aiStop = true;
        }
    }
    private void CheckEnemyAIStop()
    {
        if (enemyAIStop) return;
        if (sumAllyValues > sumAiValues && sumAllyValues < 42) return;
        if (sumAiValues > sumAllyValues && sumAiValues >= 37) aiStop = true;
        if (sumAllyValues <= sumAiValues && myStop && alliedAIStop) aiStop = true;
        if (enemyaiValues >= 20) enemyAIStop = true;
        else if (enemyaiValues == 19)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 99) enemyAIStop = true;
        }
        else if (enemyaiValues == 18)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) enemyAIStop = true;
        }
        else if (enemyaiValues == 17)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 90) enemyAIStop = true;
        }
    }

    private void ChecarFim()
    {
        if ((sumAiValues == 42 && aiValues == 21 && enemyaiValues == 21) || (sumAllyValues < sumAiValues && myStop && alliedAIStop && enemyAIStop && aiStop) || (myValues > 21 || alliedaiValues > 21))
        {
            end = true;
            LoadingGif.SetActive(false);
            FimDaPartida.instance.Lose();
            PlayerController.instance.RemoveAmountMoney(bet);
            StartCoroutine(Load());
        }
        else if ((sumAllyValues == 42 && myValues == 21 && alliedaiValues == 21) || (sumAllyValues > sumAiValues && myStop && alliedAIStop && enemyAIStop && aiStop) || (aiValues > 21 || enemyaiValues > 21))
        {
            end = true;
            LoadingGif.SetActive(false);
            FimDaPartida.instance.Win(bet * 2);
            PlayerController.instance.AddAmountMoney(bet, 4);
            StartCoroutine(Load());
        }
        else if ((sumAllyValues == sumAiValues && myStop && alliedAIStop && enemyAIStop && aiStop))
        {
            end = true;
            LoadingGif.SetActive(false);
            FimDaPartida.instance.Draw(bet);
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        continuarBtn.interactable = true;
        btnPartida.SetActive(false);
        LoadingGif.SetActive(true);
        yield return new WaitForSecondsRealtime(.5f);
        LoadingGif.SetActive(false);
        btnNovaPartida.SetActive(true);
    }
    public void Reset()
    {
        sumAiValues = 0;
        sumAllyValues = 0;
        alliedAIAtualSpot.gameObject.SetActive(false);
        alliedAIAtualSpot = null;
        alliedaiValues = 0;
        alliedAIQtdCards = 2;
        alliedAIStop = false;

        aiAtualSpot.gameObject.SetActive(false);
        aiAtualSpot = null;
        aiValues = 0;
        aiQtdCards = 2;
        aiStop = false;

        enemyAIAtualSpot.gameObject.SetActive(false);
        enemyAIAtualSpot = null;
        enemyaiValues = 0;
        enemyAIQtdCards = 2;
        enemyAIStop = false;

        myAtualSpot.gameObject.SetActive(false);
        myAtualSpot = null;
        myValues = 0;
        myQtdCards = 2;
        end = myStop = false;

        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);
        FimDaPartida.instance.CloseAll();
        myCardsValue.OnValueChanged(myValues);
        alliedAICardsValue.OnValueChanged(myValues);
        totalEnemyAICardsValue.OnValueChanged(sumAiValues);

        aiCardsValue.OnValueChanged(aiValues);
        totalAllyCardsValue.OnValueChanged(sumAllyValues);
        enemyAICardsValue.OnValueChanged(aiValues);

        btnNovaPartida.SetActive(false);
        btnInicio.SetActive(true);
    }
}
