using UnityEngine;
using UnityEngine.UI; // 👈 Necesario para Image

public class RegaderaPrueba : ObjetoInteractuable
{
    [Header("Capacidad de agua")]
    public int aguaMaxima = 5;
    public int aguaActual = 0;
    public int aguaRecarga = 15;

    [Header("UI")]
    public Image barra; // 👈 la imagen de tipo "Filled"

    public bool TieneAgua => aguaActual > 0;

    private void Start()
    {
        ActualizarBarraAgua();
    }

    // Recargar en la fuente
    public void RecargarAgua()
    {
        aguaActual = Mathf.Min(aguaRecarga, aguaMaxima);
        UIInventario.Instance.MostrarMensaje("Recargaste la regadera");
        ActualizarBarraAgua();
    }

    // Gastar al regar
    public bool UsarAgua()
    {
        if (aguaActual > 0)
        {
            aguaActual--;
            UIInventario.Instance.MostrarMensaje($"Agua restante: {aguaActual}");
            ActualizarBarraAgua();
            return true;
        }
        else
        {
            UIInventario.Instance.MostrarMensaje("⚠️ La regadera está vacía");
            return false;
        }
    }

    private void ActualizarBarraAgua()
    {
        if (barra != null)
        {
            barra.fillAmount = (float)aguaActual / aguaMaxima;
        }
    }
}
