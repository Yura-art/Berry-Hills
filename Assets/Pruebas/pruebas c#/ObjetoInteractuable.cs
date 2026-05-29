using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    public string nombre = "Objeto";

    private InventarioJugador inventario;

    // Posición y rotación original
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    private void Awake()
    {
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            gameObject.layer == LayerMask.NameToLayer("Interactuable"))
        {
            inventario = other.GetComponent<InventarioJugador>();

            if (inventario != null)
            {
                UIInventario.Instance.MostrarMensaje("Presiona F para recoger " + nombre);
                inventario.SetObjetoCerca(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") &&
            gameObject.layer == LayerMask.NameToLayer("Interactuable") &&
            inventario != null)
        {
            // SOLO limpiar si este objeto sigue siendo el actual
            if (inventario.objetoCerca == this)
            {
                UIInventario.Instance.MostrarMensaje("");
                inventario.SetObjetoCerca(null);
            }

            inventario = null;
        }
    }

    public virtual void Usar() { }

    public void Recoger()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
    }

    public void Soltar()
    {
        transform.SetParent(null);

        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;

        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = true;
            //rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (col != null) col.enabled = true;

        gameObject.SetActive(true);
    }
}