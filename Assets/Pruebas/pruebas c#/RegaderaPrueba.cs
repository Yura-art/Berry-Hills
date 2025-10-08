using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RegaderaPrueba : ObjetoInteractuable
{
    [Header("Capacidad de agua")]
    public int aguaMaxima = 5;
    public int aguaActual = 0;
    public int aguaRecarga = 15;

    [Header("UI")]
    public Image barra;

    [Header("Recarga")]
    public float tiempoRecarga = 2f; // segundos que tarda en recargar

    public bool TieneAgua => aguaActual > 0;

    private void Start()
    {
        ActualizarBarraAgua();
    }

    // Inicia el proceso de recarga
    public bool RecargarAgua()
    {
        // ❌ Si ya está llena, no recargar
        if (aguaActual >= aguaMaxima)
        {
            UIInventario.Instance.MostrarMensaje("La regadera ya está llena");
            return false; // <- no recargó
        }

        MovimientoJugador jugador = FindObjectOfType<MovimientoJugador>();
        if (jugador != null)
        {
            jugador.StartCoroutine(ProcesoRecarga(jugador));
        }

        return true; // <- recarga iniciada
    }


    private IEnumerator ProcesoRecarga(MovimientoJugador jugador)
    {
        // Bloquea movimiento
        jugador.puedeMover = false;
        UIInventario.Instance.MostrarMensaje("Recargando la regadera...");

        // Espera unos segundos
        yield return new WaitForSeconds(tiempoRecarga);

        // Recarga agua
        aguaActual = Mathf.Min(aguaRecarga, aguaMaxima);
        ActualizarBarraAgua();
        AudioManager.instance.DetenerFuenteAgua();

        // Devuelve control al jugador
        jugador.puedeMover = true;
        UIInventario.Instance.MostrarMensaje("");
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
            UIInventario.Instance.MostrarMensaje("La regadera está vacía");
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
