using UnityEngine;

public class ZonaSiembra : MonoBehaviour
{
    private bool enRango = false;
    private bool ocupada = false;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        // 👇 Solo permite acción si el jugador está dentro de la zona
        if (!enRango) return;
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                InventarioJugador inventario = FindObjectOfType<InventarioJugador>();

                if (inventario != null && inventario.ObjetoEnMano is PruebaBolsa bolsa)
                {
                    GameObject prefab = bolsa.ObtenerPrefab();

                    if (prefab != null)
                    {
                        // Instanciamos la planta
                        Instantiate(prefab, transform.position, Quaternion.identity);

                        // Sacamos la bolsa del inventario
                        inventario.SoltarObjeto(inventario.SlotActivo);

                        // La regresamos a su lugar inicial
                        bolsa.VolverASitio();

                        // Marcamos ocupada y desactivamos el collider
                        ocupada = true;
                        if (col != null)
                            col.enabled = false;

                        // Limpiamos mensaje porque ya no se puede usar
                        UIInventario.Instance.MostrarMensaje("");
                    }
                }
        
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BolsaSemillas"))
        {
            enRango = true;

            if (!ocupada)
                UIInventario.Instance.MostrarMensaje("Presiona E para sembrar");
            else
                UIInventario.Instance.MostrarMensaje("Ya está ocupada");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BolsaSemillas"))
        {
            enRango = false;
            UIInventario.Instance.MostrarMensaje(""); // 🔑 Se limpia siempre al salir
        }
    }

    // 👇 Llamar cuando quieras liberar la maceta (ej: al cosechar la planta)
    public void LiberarMaceta()
    {
        ocupada = false;

        if (col != null)
            col.enabled = true;

        // 🔑 Buscar al jugador y limpiar el objeto cerca
        InventarioJugador inventario = FindObjectOfType<InventarioJugador>();
        if (inventario != null)
        {
            inventario.SetObjetoCerca(null);
        }

        UIInventario.Instance.MostrarMensaje("La maceta está lista para sembrar de nuevo 🌱");
    }

}
