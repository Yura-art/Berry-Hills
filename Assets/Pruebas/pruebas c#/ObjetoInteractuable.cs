using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    public string nombre = "Objeto";      // Nombre del objeto
    //public Sprite icono;                  // Icono para mostrar en la UI

    private InventarioJugador inventario; // Referencia al inventario del jugador cercano

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger detectado con: " + other.name);

        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Interactuable"))
        {
            //Debug.Log("Jugador entró al trigger del objeto: " + nombre);
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

    public virtual void Usar()
    {
        //UIInventario.Instance.MostrarMensaje("Usaste " + nombre);
    }

    public void Recoger()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
    }

    public void Soltar(Vector3 posicion)
    {
        transform.position = posicion;

        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = false;
            Invoke(nameof(KinematicActivo), 1f);
            rb.velocity = Vector3.zero;
        }

        if (col != null) col.enabled = true;

        gameObject.SetActive(true);
    }

    public void KinematicActivo()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}


