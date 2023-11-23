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

    [SerializeField] private GameObject LoadingGif;
    [SerializeField] private GameObject myLocalCards;
    [SerializeField] private GameObject[] aiLocalCards;
    [SerializeField] private Transform myAtualSpot = null;
    [SerializeField] private Transform[] aiAtualSpot;
    [SerializeField] private CardsValue myCardsValue;
    [SerializeField] private CardsValue[] aiCardsValue;
    [SerializeField] private float bet;
    [SerializeField] private InputMoney inputMoney;
    [SerializeField] private GameObject btnInicio;
    [SerializeField] private GameObject btnPartida;
    [SerializeField] private GameObject btnNovaPartida;
    [SerializeField] private bool[] aiStop;
    [SerializeField] private bool myStop;
    [SerializeField] private bool[] aiLose;
    [SerializeField] private short ganhou;
    [SerializeField] private bool end;
    [SerializeField] private int myQtdCards = 2;
    [SerializeField] private int[] aiQtdCards;
    [SerializeField] private int totalAICount;
    [SerializeField] private Button continuarBtn;

    [field: SerializeField]
    public int MyValues { get; private set; }
    [field: SerializeField]
    public int[] AiValues { get; private set; }

    void Start()
    {
        MyValues = 0;
        AiValues = new int[totalAICount];
        aiAtualSpot = new Transform[totalAICount];
        aiStop = new bool[totalAICount];
        aiLose = new bool[totalAICount];
        aiQtdCards = new int[totalAICount];

        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);

        myAtualSpot = null;
        myQtdCards = 2;
        for (int aiIndex = 0; aiIndex < totalAICount; aiIndex++)
        {
            aiAtualSpot[aiIndex] = null;
            aiQtdCards[aiIndex] = 2;
            aiStop[aiIndex] = false;
            aiLose[aiIndex] = false;
            AiValues[aiIndex] = 0;
        }
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
            MyCardsChange();
            for (int aiIndex = 0; aiIndex < totalAICount; aiIndex++)
            {
                AiCardsChange(aiIndex);
            }
            inputMoney.ResetValue();
            btnInicio.SetActive(false);
            btnPartida.SetActive(true);

            for (int aiIndex = 0; aiIndex < totalAICount; aiIndex++)
            {
                ChecarTurno(aiIndex);
            }
        }
    }

    public void ButtonContinue()
    {
        MyCardsChange();
        for (int aiIndex = 0; aiIndex < totalAICount; aiIndex++)
        {
            ChecarTurno(aiIndex);
            if (!end)
            {
                StartCoroutine(AiPlay());
            }
        }
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
        for (int aiIndex = 0; aiIndex < totalAICount; aiIndex++)
        {
            if (aiLose[aiIndex] || end) continue;
            btnPartida.SetActive(false);
            LoadingGif.SetActive(true);
            //desativar o botão e ativar o waiting
            yield return new WaitForSecondsRealtime(.5f);
            //ativar o botão e desativar o waiting
            AiCardsChange(aiIndex);
            ChecarTurno(aiIndex);
        }
        if (!myStop && !end)
        {
            btnPartida.SetActive(true);

            LoadingGif.SetActive(false);
        }
        if (myStop && !end && !ChecarDerrotaIA()) StartCoroutine(AiPlay());
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
                    MyValues = MyValues + 11 > 21 ? MyValues + 1 : MyValues + 11;
                }
                else
                {
                    MyValues += aux.Numero;
                }
                myAtualSpot.GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        myCardsValue.OnValueChanged(MyValues);

        myQtdCards++;
    }

    public void AiCardsChange(int aiIndex)
    {
        CheckAiStop(aiIndex);

        if (aiStop[aiIndex]) return;

        Transform auxSpot = null;
        if (aiAtualSpot[aiIndex] != null)
        {
            auxSpot = aiAtualSpot[aiIndex];
            auxSpot.gameObject.SetActive(false);
        }
        aiAtualSpot[aiIndex] = aiLocalCards[aiIndex].GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.gameObject.CompareTag(aiQtdCards[aiIndex] + " Cards"));

        aiAtualSpot[aiIndex].gameObject.SetActive(true);

        for (int i = 0; i < aiQtdCards[aiIndex]; i++)
        {
            if (auxSpot != null && i < aiQtdCards[aiIndex] - 1)
            {
                aiAtualSpot[aiIndex].GetComponentsInChildren<Image>()[i].sprite = auxSpot.GetComponentsInChildren<Image>()[i].sprite;
            }
            else
            {
                Cards aux = cardsStack.Pop();
                if (aux.IsAs)
                {
                    AiValues[aiIndex] = AiValues[aiIndex] + 11 > 21 ? AiValues[aiIndex] + 1 : AiValues[aiIndex] + 11;
                }
                else
                {
                    AiValues[aiIndex] += aux.Numero;
                }
                aiAtualSpot[aiIndex].GetComponentsInChildren<Image>()[i].sprite = aux.Image;
            }
        }
        aiCardsValue[aiIndex].OnValueChanged(AiValues[aiIndex]);

        aiQtdCards[aiIndex]++;
    }

    private void CheckAiStop(int aiIndex)
    {
        if (aiStop[aiIndex]) return;
        if (MyValues > AiValues[aiIndex] && MyValues < 21) return;
        if (MyValues < AiValues[aiIndex] && myStop) aiStop[aiIndex] = true;
        else if (AiValues[aiIndex] == 20) aiStop[aiIndex] = true;
        else if (AiValues[aiIndex] == 19)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) aiStop[aiIndex] = true;
        }
        else if (AiValues[aiIndex] == 18)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 98) aiStop[aiIndex] = true;
        }
        else if (AiValues[aiIndex] == 17)
        {
            System.Random r = new System.Random();

            int number = r.Next(1, 101);
            if (number > 90) aiStop[aiIndex] = true;
        }
    }

    private void ChecarTurno(int aiIndex)
    {
        if(AiValues[aiIndex] == 21 || MyValues > 21)
        {
            Derrota();
            return;
        }
        else if (MyValues == 21)
        {
            Vitoria();
            return;
        }
        else if (AiValues[aiIndex] > 21)
        {
            AiValues[aiIndex] = 0;
            aiStop[aiIndex] = aiLose[aiIndex] = true;
        }
        if (ChecarDerrotaIA())
        {
            Vitoria();
            return;
        }
        else if (myStop)
        {
            if (AllStop())
            {
                if (AiValues.Max() > MyValues)
                {
                    Derrota();
                    return;
                }
                else if (AiValues.Max() < MyValues)
                {
                    Vitoria();
                    return;
                }
                else
                {
                    Empate();
                    return;
                }
            }
        }
    }
    private bool ChecarDerrotaIA()
    {
        end = true;
        foreach (bool lose in aiLose)
        {
            if (!lose)
            {
                end = false;
                return end;
            }
        }
        return end;
    }
    private bool AllStop()
    {
        foreach(bool stop in aiStop)
        {
            if (!stop) return false;
        }
        return true;
    }
    private void Vitoria()
    {
        end = true;
        LoadingGif.SetActive(false);
        FimDaPartida.instance.Win(bet * 2);
        PlayerController.instance.AddAmountMoney(bet);
        StartCoroutine(Load());
    }
    private void Derrota()
    {
        end = true;
        LoadingGif.SetActive(false);
        FimDaPartida.instance.Lose();
        PlayerController.instance.RemoveAmountMoney(bet);
        StartCoroutine(Load());
    }
    private void Empate()
    {
        end = true;
        LoadingGif.SetActive(false);
        FimDaPartida.instance.Draw(bet);
        StartCoroutine(Load());
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
        myAtualSpot.gameObject.SetActive(false);

        for (int i = 0; i < totalAICount; i++)
        {
            aiAtualSpot[i].gameObject.SetActive(false);
            aiAtualSpot[i] = null;
            AiValues[i] = 0;
            aiQtdCards[i] = 2;
            aiStop[i] = false;
            aiLose[i] = false;
        }

        myAtualSpot = null;
        MyValues = 0;
        myQtdCards = 2;
        end = myStop = false;
        Shuffle(cards);
        cardsStack = new Stack<Cards>(cards);
        FimDaPartida.instance.CloseAll();
        myCardsValue.OnValueChanged(MyValues);

        for (int i = 0; i < totalAICount; i++)
        {
            aiCardsValue[i].OnValueChanged(AiValues[i]);
        }

        btnNovaPartida.SetActive(false);
        btnInicio.SetActive(true);
    }
}
