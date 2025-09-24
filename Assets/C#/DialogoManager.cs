using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Eventos del diálogo")]
    [Tooltip("Objetos que se activarán en cierto índice del diálogo")]
    public List<GameObject> objetosAActivar;
    public List<int> indicesActivar;

    [Tooltip("Objetos que se desactivarán en cierto índice del diálogo")]
    public List<GameObject> objetosADesactivar;
    public List<int> indicesDesactivar;

    void Start()
    {
        panelDialogo.SetActive(false);
        botonSiguiente.onClick.AddListener(MostrarSiguienteFrase);
    }

    void Update()
    {
        if (!panelDialogo.activeSelf) return;

        // ⌨️ Avanzar diálogo con Espacio o Enter
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            MostrarSiguienteFrase();
        }
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

            // detener sonido para que no se solapen
            AudioManager.instance.DetenerDialogo();

            // limpiar texto
            textoDialogo.text = "";

            // iniciar escritura letra por letra
            escribiendo = StartCoroutine(EscribirTexto(frases[indice]));

            // ✅ Revisar activaciones / desactivaciones
            RevisarEventosDialogo(indice);

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

            // 🔊 sonido por letra (tipo beep retro)
            AudioManager.instance.ReproducirDialogo();

            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    void RevisarEventosDialogo(int indiceActual)
    {
        // Activar objetos si toca
        for (int i = 0; i < indicesActivar.Count; i++)
        {
            if (indicesActivar[i] == indiceActual && i < objetosAActivar.Count)
            {
                objetosAActivar[i].SetActive(true);
            }
        }

        // Desactivar objetos si toca
        for (int i = 0; i < indicesDesactivar.Count; i++)
        {
            if (indicesDesactivar[i] == indiceActual && i < objetosADesactivar.Count)
            {
                objetosADesactivar[i].SetActive(false);
            }
        }
    }

    void CerrarDialogo()
    {
        if (escribiendo != null)
            StopCoroutine(escribiendo);

        // detener cualquier sonido de diálogo
        AudioManager.instance.DetenerDialogo();

        panelDialogo.SetActive(false);
        Debug.Log("📖 Diálogo terminado");
        UIManagerMensajes.instance.MostrarMensaje("");
    }
}
