using UnityEngine;

public class ZonaSiembra : MonoBehaviour
{
    [Header("Opciones de siembra")]
    [SerializeField] private string tagBolsa = "BolsaSemillas";

    private bool enRango = false;
    public bool ocupada = false;
    private InventarioJugador jugador;

    private void Update()
    {
        if (!enRango || jugador == null) return;

        // Mensaje dinámico según estado
        if (ocupada)
        {
            return; // Planta ocupada, no mostramos mensaje de siembra
        }

        if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagBolsa))
        {
            UIInventario.Instance.MostrarMensaje("Presiona E para sembrar");

            if (Input.GetKeyDown(KeyCode.E))
                Sembrar(jugador.ObjetoEnMano);
        }
        else
        {
            UIInventario.Instance.MostrarMensaje(""); // Limpia mensaje si no hay bolsa
        }
    }

    private void Sembrar(ObjetoInteractuable bolsa)
    {
        if (bolsa == null) return;

        PruebaBolsa bolsaSemilla = bolsa as PruebaBolsa;
        if (bolsaSemilla == null) return;

        GameObject prefab = bolsaSemilla.ObtenerPrefab();
        if (prefab == null) return;

        // 🔑 Activar animación de interacción
        Animator anim = jugador.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetBool("Interactuando", true);

        // Instanciar planta
        Vector3 posicionSemilla = transform.position + Vector3.up * 1.3f;
        GameObject semillaObj = Instantiate(prefab, posicionSemilla, Quaternion.identity);

        if (semillaObj.TryGetComponent(out CrecimientoPrueba crecimiento))
        {
            crecimiento.DarZonaSiembra(this);
        }

        // Sacar del inventario y devolver bolsa
        jugador.SoltarObjeto(jugador.SlotActivo);
        bolsaSemilla.VolverASitio();

        // Bloquear la maceta hasta cosecha
        ocupada = true;

        // Limpiar mensaje
        UIInventario.Instance.MostrarMensaje("");
        AudioManager.instance.ReproducirSonido(AudioManager.instance.sembrar);

        // 🔑 Desactivar animación para que no se quede pegada
        if (anim != null)
            anim.SetBool("Interactuando", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            jugador = inv;
            enRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null && inv == jugador)
        {
            jugador = null;
            enRango = false;
            UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Llamar desde CrecimientoPrueba cuando la planta esté lista para cosecha
    public void LiberarMaceta()
    {
        ocupada = false;
    }
}
