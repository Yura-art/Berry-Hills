using UnityEngine;
using TMPro;

public class UIManagerMensajes : MonoBehaviour
{
    public static UIManagerMensajes instance;

    [Header("UI")]
    public TextMeshProUGUI textoMensajes;
    public TextMeshProUGUI textoAdvertencias;

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
    public void MostrarAdvertencia(string mensaje, float duracion = 2f)
    {
        if (textoAdvertencias != null)
        {
            string mensajeFormateado = mensaje.Replace(". ", ".\n");
            textoAdvertencias.text = mensajeFormateado;
            CancelInvoke(nameof(LimpiarAdvertencia));
            Invoke(nameof(LimpiarAdvertencia), duracion);
        }
    }


    public void LimpiarMensaje()
    {
        UIManagerMensajes.instance.MostrarMensaje("");
    }
    public void LimpiarAdvertencia()
    {
        UIManagerMensajes.instance.MostrarAdvertencia("");
    }
}
