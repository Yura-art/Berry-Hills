using System.Collections;
using UnityEngine;

public class CrecimientoPlanta : ObjetoLlevable, IInteractuableF
{
    private Regadera regaderaEnZona;
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

    private void Start()
    {
        puedeCargar = false;
        if (objetoEspera != null)
            objetoEspera.SetActive(false);
    }

    public override void xD()
    {
        Debug.Log("Crecimiento planta");
    }

    public void InteractuarClick(GameObject interactor) // ✅ solo F
    {
        if (regaderaEnZona != null)
        {
            regaderaEnZona.Regar(this);
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
            if (!puedeCargar)
            {
                puedeCargar = true;

                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.mass = 1f;
                rb.useGravity = true;
                rb.isKinematic = false;

                BoxCollider boxColliderExistente = GetComponent<BoxCollider>();
                if (boxColliderExistente != null)
                    Destroy(boxColliderExistente);

                BoxCollider nuevoBoxCollider = gameObject.AddComponent<BoxCollider>();
                nuevoBoxCollider.size = new Vector3(3f, 3f, 3f);
                nuevoBoxCollider.center = Vector3.zero;
            }
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

    private void OnTriggerEnter(Collider other)
    {
        var regadera = other.GetComponent<Regadera>();
        if (regadera != null)
        {
            regaderaEnZona = regadera;
            Debug.Log("Regadera detectada en la planta.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var regadera = other.GetComponent<Regadera>();
        if (regadera != null && regadera == regaderaEnZona)
        {
            regaderaEnZona = null;
            Debug.Log("Regadera salió de la planta.");
        }
    }
}
