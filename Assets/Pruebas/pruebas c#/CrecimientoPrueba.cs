using UnityEngine;
using System.Collections;

public class CrecimientoPrueba : MonoBehaviour
{
    [Header("Crecimiento")]
    public int etapaActual = 0;
    public int etapaMaxima = 3;
    public float aguaPorEtapa = 5f;
    public float tiempoEsperaEntreFases = 3f;
    public Animator animator;
    public GameObject objetoEspera; // Indicador de cooldown

    private float agua = 0f;
    private bool enCooldown = false;

    // Riego
    private bool regaderaEnRango = false;
    private RegaderaPrueba regadera;

    private ZonaSiembra zonaSiembra;

    private void Awake()
    {
        // Busca la ZonaSiembra en el objeto padre (asumiendo que la planta es hijo de la maceta)
        zonaSiembra = GetComponentInParent<ZonaSiembra>();
        if (zonaSiembra == null)
            Debug.LogWarning("No se encontró ZonaSiembra en los padres");
    }
    public void DarZonaSiembra(ZonaSiembra zona)
    {
        zonaSiembra = zona;
    }

    void Update()
    {
        if (etapaActual >= etapaMaxima) return;

        if (regaderaEnRango && Input.GetKeyDown(KeyCode.E))
        {
            if (!enCooldown && regadera != null && regadera.UsarAgua())
            {
                RecibirAgua(1f);
            }
            else if (enCooldown)
            {
                UIInventario.Instance.MostrarMensaje("La planta está absorbiendo agua, espera un momento");
            }
        }
    }

    public void RecibirAgua(float cantidad)
    {
        if (etapaActual >= etapaMaxima || enCooldown) return;

        agua += cantidad;

        if (agua >= aguaPorEtapa)
        {
            agua = 0;
            Crecer();
            StartCoroutine(CooldownEntreFases());
        }
    }

    private void Crecer()
    {
        etapaActual = Mathf.Min(etapaActual + 1, etapaMaxima);
        animator.SetInteger("Etapa", etapaActual);

        if (etapaActual == etapaMaxima)
        {
            GenerarFruta();
            Debug.Log("siuuu");

            if (zonaSiembra != null)
            {
                zonaSiembra.LiberarMaceta(); // Ahora sí se libera correctamente
                Debug.Log("jijijij");
            }
        }

    }


    private IEnumerator CooldownEntreFases()
    {
        enCooldown = true;
        if (etapaActual < etapaMaxima && objetoEspera != null) objetoEspera.SetActive(true);

        yield return new WaitForSeconds(tiempoEsperaEntreFases);

        if (etapaActual < etapaMaxima && objetoEspera != null) objetoEspera.SetActive(false);
        enCooldown = false;
    }

    private void GenerarFruta()
    {
        // Añadir Rigidbody a la fruta
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        // Convertir en objeto interactuable
        ObjetoInteractuable interactuable = gameObject.AddComponent<ObjetoInteractuable>();
        interactuable.nombre = "Fruta";
        gameObject.tag = "Fruta";

        // Liberar la maceta asociada


        // Desactivar detección de regadera
        regaderaEnRango = false;
        regadera = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        RegaderaPrueba r = other.GetComponent<RegaderaPrueba>();
        if (r != null)
        {
            regaderaEnRango = true;
            regadera = r;
            UIInventario.Instance.MostrarMensaje("Presiona E para regar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RegaderaPrueba r = other.GetComponent<RegaderaPrueba>();
        if (r != null && r == regadera)
        {
            regaderaEnRango = false;
            regadera = null;
            UIInventario.Instance.MostrarMensaje("");
        }
    }
}
