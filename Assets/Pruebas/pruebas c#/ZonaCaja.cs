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
    public string tagAceptado;

    [Header("Gestor (opcional)")]
    public ManagerCajaPrueba gestor;

    private List<ObjetoInteractuable> objetosGuardados = new List<ObjetoInteractuable>();
    private bool jugadorEnRango = false;
    private InventarioJugador jugador;

    // --- Temporizador para los mensajes ---
    private float temporizadorMensaje = 0f;

    public bool EstaLlena => objetosGuardados.Count >= capacidadMaxima;

    private void Start()
    {
        ActualizarTexto();
    }

    private void Update()
    {
        ActualizarTexto();

        // 🔁 Control del temporizador para ocultar mensajes
        if (temporizadorMensaje > 0)
        {
            temporizadorMensaje -= Time.deltaTime;
            if (temporizadorMensaje <= 0)
                UIInventario.Instance.MostrarMensaje("");
        }

        // ⚙️ Comportamiento cuando el jugador está en rango
        if (jugadorEnRango && jugador != null)
        {
            if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagAceptado))
            {
                MostrarMensajeTemporal("Presiona E para guardar la fruta", 1.5f);
            }

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
            MostrarMensajeTemporal("No llevas ningún objeto para guardar.", 2.5f);
            return;
        }

        if (!objEnMano.CompareTag(tagAceptado))
        {
            MostrarMensajeTemporal($"Esta caja solo acepta '{tagAceptado}'.", 2.5f);
            return;
        }

        if (EstaLlena)
        {
            MostrarMensajeTemporal("La caja está llena.", 3f);
            return;
        }

        // ✅ Guardar el objeto
        objetosGuardados.Add(objEnMano);
        jugador.SoltarObjeto(jugador.SlotActivo);
        objEnMano.gameObject.SetActive(false);

        MostrarMensajeTemporal($"Guardaste {objEnMano.nombre}. Total: {objetosGuardados.Count}", 2f);
        AudioManager.instance.ReproducirSonido(AudioManager.instance.guardarObjeto);

        // Notificar al gestor si existe
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

            if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagAceptado))
                MostrarMensajeTemporal("Presiona E para guardar la fruta", 1.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            jugadorEnRango = false;
            jugador = null;
            UIInventario.Instance.MostrarMensaje("");
            temporizadorMensaje = 0f;
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

    // 🕒 Método auxiliar para mostrar mensajes temporales
    private void MostrarMensajeTemporal(string mensaje, float duracion)
    {
        UIInventario.Instance.MostrarMensaje(mensaje);
        temporizadorMensaje = duracion;
    }
}
