using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    public string nombre = "Objeto";

    private InventarioJugador inventario;

    // ✅ Guardamos posición y rotación original
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    private void Awake()
    {
        // ✅ Guardamos la posición inicial y rotación apenas se crea
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Interactuable"))
        {
            inventario = other.GetComponent<InventarioJugador>();
            UIInventario.Instance.MostrarMensaje("Presiona F para recoger " + nombre);
            inventario.SetObjetoCerca(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Interactuable") && inventario != null)
        {
            UIInventario.Instance.MostrarMensaje("");
            inventario.SetObjetoCerca(null);
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

    // ✅ Soltar vuelve exactamente a su posición y rotación original
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
            rb.velocity = Vector3.zero;
        }

        if (col != null) col.enabled = true;

        gameObject.SetActive(true);
    }
}
