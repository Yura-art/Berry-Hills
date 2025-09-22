using UnityEngine;
using TMPro;

public class UIManagerMensajes : MonoBehaviour
{
    public static UIManagerMensajes instance;

    [Header("UI")]
    public TextMeshProUGUI textoMensajes;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void MostrarMensaje(string mensaje)
    {
        if (textoMensajes != null)
        {
            // reemplazar puntos con saltos de línea para separar frases
            string mensajeFormateado = mensaje.Replace(". ", ".\n");
            textoMensajes.text = mensajeFormateado;
        }
    }

}
