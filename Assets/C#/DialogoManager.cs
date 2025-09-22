using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogoManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public Button botonSiguiente;

    [Header("Frases")]
    [TextArea]
    public string[] frases;
    private int indice = 0;

    [Header("Velocidad de texto")]
    public float velocidadEscritura = 0.05f; // tiempo entre letras

    private Coroutine escribiendo; // referencia para detener la corutina

    void Start()
    {
        panelDialogo.SetActive(false);
        botonSiguiente.onClick.AddListener(MostrarSiguienteFrase);
    }

    public void IniciarDialogo()
    {
        indice = 0;
        panelDialogo.SetActive(true);

        // 🔊 Sonido al iniciar diálogo
        AudioManager.instance.ReproducirDialogo();

        MostrarSiguienteFrase();
    }

    public void MostrarSiguienteFrase()
    {
        if (indice < frases.Length)
        {
            // detener escritura previa si existía
            if (escribiendo != null)
                StopCoroutine(escribiendo);

            // limpiar texto
            textoDialogo.text = "";

            // iniciar escritura letra por letra
            escribiendo = StartCoroutine(EscribirTexto(frases[indice]));
            indice++;
        }
        else
        {
            CerrarDialogo();
        }
    }

    IEnumerator EscribirTexto(string frase)
    {
        foreach (char letra in frase.ToCharArray())
        {
            textoDialogo.text += letra;

            // 🔊 opcional: sonido por letra (tipo beep retro)
            AudioManager.instance.ReproducirDialogo();

            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    void CerrarDialogo()
    {
        panelDialogo.SetActive(false);
        Debug.Log("📖 Diálogo terminado");
    }
}
