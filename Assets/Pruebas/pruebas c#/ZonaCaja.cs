using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZonaCaja : MonoBehaviour
{
    [Header("Capacidad")]
    public int capacidadMaxima = 5;

    [Header("Texto de UI")]
    public TextMeshProUGUI capacidadText;

    [Header("Tag aceptado")]
    public string tagAceptado = "Fruta";

    [Header("Manager (opcional)")]
    public ManagerCajaPrueba gestor;

    private List<ObjetoInteractuable> objetosGuardados = new List<ObjetoInteractuable>();
    private bool jugadorEnRango = false;
    private InventarioJugador jugador;

    public bool EstaLlena => objetosGuardados.Count >= capacidadMaxima;

    private void Start() => ActualizarTexto();

    private void Update()
    {
        // Actualiza el texto de capacidad
        ActualizarTexto();

        // 🔑 Actualiza dinámicamente el mensaje mientras el jugador está en el trigger
        if (jugadorEnRango && jugador != null)
        {
            if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagAceptado))
            {
                UIInventario.Instance.MostrarMensaje("Presiona E para guardar la fruta");
            }
            else
            {
                UIInventario.Instance.MostrarMensaje(""); // No tiene objeto válido en mano
            }

            // Guardar con E
            if (Input.GetKeyDown(KeyCode.E))
                GuardarObjeto();
        }
    }

    private void GuardarObjeto()
    {
        if (jugador == null) return;

        ObjetoInteractuable objEnMano = jugador.ObjetoEnMano;
        if (objEnMano == null)
        {
            UIInventario.Instance.MostrarMensaje("No llevas ningún objeto para guardar.");
            return;
        }

        if (!objEnMano.CompareTag(tagAceptado))
        {
            UIInventario.Instance.MostrarMensaje($"Esta caja solo acepta objetos con tag '{tagAceptado}'.");
            return;
        }

        if (EstaLlena)
        {
            UIInventario.Instance.MostrarMensaje("La caja está llena.");
            return;
        }

        // Guardar
        objetosGuardados.Add(objEnMano);
        jugador.SoltarObjeto(jugador.SlotActivo);
        objEnMano.gameObject.SetActive(false);

        UIInventario.Instance.MostrarMensaje($"Guardaste {objEnMano.nombre}. Total: {objetosGuardados.Count}");

        // Notificar al gestor
        if (gestor != null)
            gestor.VerificarCajasLlenas();
    }

    private void OnTriggerEnter(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            jugadorEnRango = true;
            jugador = inv;

            // Mensaje inicial según objeto en mano
            if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagAceptado))
                UIInventario.Instance.MostrarMensaje("Presiona E para guardar la fruta");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            jugadorEnRango = false;
            jugador = null;
            UIInventario.Instance.MostrarMensaje(""); // limpiar mensaje
        }
    }

    public void ActualizarTexto()
    {
        if (capacidadText != null)
            capacidadText.text = objetosGuardados.Count + " / " + capacidadMaxima;
    }

    public void ReiniciarCaja()
    {
        objetosGuardados.Clear();
        ActualizarTexto();
    }

    public List<ObjetoInteractuable> ObtenerObjetosGuardados() => objetosGuardados;
}
