using UnityEngine;
using TMPro;

public class ControlTiempo : MonoBehaviour
{
    public float[] tiemposPorFase;
    private int faseActual = 0;

    private float tiempoRestante;
    private bool enCuentaAtras = false;

    public TMP_Text textoTiempo;
    public TMP_Text textoPuntaje;

    public GameObject panelPerdiste;

    private int puntajeTotal = 0;

    private void Start()
    {
        IniciarTiempo();
        MostrarPuntaje();
        panelPerdiste.SetActive(false);
    }

    private void Update()
    {
        if (!enCuentaAtras) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            enCuentaAtras = false;
            Perder();
        }

        MostrarTiempo();
    }

    public void IniciarTiempo()
    {
        if (faseActual < tiemposPorFase.Length)
            tiempoRestante = tiemposPorFase[faseActual];
        else
            tiempoRestante = tiemposPorFase[tiemposPorFase.Length - 1];

        enCuentaAtras = true;
        MostrarTiempo();
    }

    public void DetenerTiempo()
    {
        enCuentaAtras = false;
    }

    public void Ganar()
    {
        int puntosFase = Mathf.CeilToInt(tiempoRestante);
        puntajeTotal += puntosFase;

        MostrarPuntaje();
        DetenerTiempo();

        faseActual++;
        IniciarTiempo();
    }

    private void Perder()
    {
        panelPerdiste.SetActive(true);
        Time.timeScale = 0f;

    }

    private void MostrarTiempo()
    {
        if (textoTiempo != null)
        {
            textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();
        }
    }

    private void MostrarPuntaje()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = puntajeTotal.ToString();
        }
    }
}
