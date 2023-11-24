using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ComprarFicha : MonoBehaviour
{
    private const string FORMATO_DATA = "yyyy-MM-dd HH:mm:ss";
    [SerializeField] private Button botao;
    private string CHAVE_ULTIMO_CLIQUE;

    public TextMeshProUGUI contadorText;
    private void Awake()
    {
        botao = GetComponent<Button>();
    }
    private void Start()
    {
        CHAVE_ULTIMO_CLIQUE = PlayerController.instance.player.Name;
        AtualizarBotao();
        Debug.Log("a");
    }

    private void Update()
    {
        AtualizarContador();
    }

    public void OnBtnClick()
    {
        if (PodeClicarNoBotaoHoje())
        {
            PlayerController.instance.AddAmountMoney(10000, 0);
            PlayerPrefs.SetString(CHAVE_ULTIMO_CLIQUE, DateTime.Now.ToString(FORMATO_DATA));

            AtualizarBotao();
        }
    }

    private bool PodeClicarNoBotaoHoje()
    {
        string dataUltimoCliqueSalva = PlayerPrefs.GetString(CHAVE_ULTIMO_CLIQUE, "");

        if (string.IsNullOrEmpty(dataUltimoCliqueSalva))
        {
            return true;
        }
        else
        {
            DateTime dataUltimoClique = DateTime.ParseExact(dataUltimoCliqueSalva, FORMATO_DATA, null);
            DateTime hoje = DateTime.Now;

            return (hoje - dataUltimoClique).Days >= 1;
        }
    }

    private void AtualizarBotao()
    {
        if (PodeClicarNoBotaoHoje())
        {
            botao.interactable = true;

            contadorText.text = "Disponível";
        }
        else
        {
            botao.interactable = false;
        }
    }

    private void AtualizarContador()
    {
        if (!PodeClicarNoBotaoHoje())
        {
            DateTime dataUltimoClique = DateTime.ParseExact(PlayerPrefs.GetString(CHAVE_ULTIMO_CLIQUE, ""), FORMATO_DATA, null);
            DateTime proximoClique = dataUltimoClique.AddDays(1);
            TimeSpan tempoRestante = proximoClique - DateTime.Now;

            contadorText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", tempoRestante.Hours, tempoRestante.Minutes, tempoRestante.Seconds);
        }
        else
        {
            contadorText.text = "Cuidado! O tempo de espera é de 1 dia para comprar novamente!";
        }
    }
}
