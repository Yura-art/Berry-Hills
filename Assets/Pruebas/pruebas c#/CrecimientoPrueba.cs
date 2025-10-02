using System.Collections;
using UnityEngine;


public class CrecimientoPrueba : MonoBehaviour
{
    private float agua = 0f;

    [Header("Crecimiento")]
    public float aguaPorEtapa = 5f;
    public int etapaActual = 0;
    public int etapaMaxima = 3;
    public Animator animator;

    [Header("Cooldown entre fases")]
    public float tiempoEsperaEntreFases = 3f;
    public GameObject objetoEspera;

    private bool enCooldown = false;

    // 👇 extra para el riego
    private bool regaderaEnRango = false;
    private RegaderaPrueba regadera;



    void Update()
    {
        // Si la planta está madura, ya no se puede regar
        if (etapaActual >= etapaMaxima) return;

        // Solo si la regadera está en rango y el jugador pulsa E
        if (regaderaEnRango && Input.GetKeyDown(KeyCode.E))
        {
            if (!enCooldown && regadera.UsarAgua())
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
        if (etapaActual >= etapaMaxima) return;
        if (enCooldown) return;

        agua += cantidad;

        if (agua >= aguaPorEtapa && etapaActual < etapaMaxima)
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

        if (AudioManager.instance != null && AudioManager.instance.cosechar != null)
        {
            AudioManager.instance.ReproducirSonido(AudioManager.instance.cosechar);
        }

        if (etapaActual == etapaMaxima)
        {
            // Desactivamos detección de regadera
            regaderaEnRango = false;
            regadera = null;

            // Rigidbody para el fruto
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // no empuja al jugador

            // Objeto interactuable
            gameObject.AddComponent<ObjetoInteractuable>().nombre = "Fruta";
        }



    }

    private IEnumerator CooldownEntreFases()
    {
        enCooldown = true;

        if (etapaActual < etapaMaxima && objetoEspera != null)
            objetoEspera.SetActive(true);

        yield return new WaitForSeconds(tiempoEsperaEntreFases);

        if (etapaActual < etapaMaxima && objetoEspera != null)
            objetoEspera.SetActive(false);

        enCooldown = false;
    }

    // 👇 Detecta regadera en el trigger
    void OnTriggerEnter(Collider other)
    {
        var r = other.GetComponent<RegaderaPrueba>();
        if (r != null)
        {
            regaderaEnRango = true;
            UIInventario.Instance.MostrarMensaje("Presiona E para regar");
            regadera = r;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var r = other.GetComponent<RegaderaPrueba>();
        if (r != null)
        {
            regaderaEnRango = false;
            UIInventario.Instance.MostrarMensaje("");
            regadera = null;
        }
    }
}
