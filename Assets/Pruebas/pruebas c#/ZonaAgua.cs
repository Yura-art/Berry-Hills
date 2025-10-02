using UnityEngine;

public class ZonaAgua : MonoBehaviour
{
    private bool enRango = false;
    private RegaderaPrueba regadera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Regadera"))
        {
            enRango = true;
            regadera = other.GetComponent<RegaderaPrueba>();
            UIInventario.Instance.MostrarMensaje("Presiona E para llenar la regadera 💧");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Regadera"))
        {
            enRango = false;
            regadera = null;
            UIInventario.Instance.MostrarMensaje("");
        }
    }

    private void Update()
    {
        if (enRango && Input.GetKeyDown(KeyCode.E) && regadera != null)
        {
            regadera.RecargarAgua();
        }
    }
}
