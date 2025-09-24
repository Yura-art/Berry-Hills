using UnityEngine;
using System.Collections;

public class BolsaSemillas : ObjetoLlevable, IInteractuableF
{
    [Header("Tipo de planta a sembrar")]
    public Planta.TipoPlanta tipoAsembrar;

    [Header("Prefabs de plantas")]
    public GameObject prefabCerezas;
    public GameObject prefabBananos;
    public GameObject prefabManzanas;

    private Transform puntoSiembra;
    private bool enZonaSiembra = false;
    private bool yaSembrado = false;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Transform padreInicial;

    private void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        padreInicial = transform.parent;
    }

    public override void xD()
    {
        Debug.Log("Bolsa semillas");
    }

    public override void Interactuar(GameObject jugador)
    {
        base.Interactuar(jugador); // ✅ sigue siendo con E
    }

    public void InteractuarClick(GameObject interactor) // ✅ con F
    {
        Debug.Log("interactua");
        Sembrar();
    }

    public void Sembrar()
    {
        if (!enZonaSiembra && puntoSiembra == null)
        {
            UIManagerMensajes.instance.MostrarAdvertencia("No estas en una zona de siembra");
            return;
        }

        if (yaSembrado)
        {
            UIManagerMensajes.instance.MostrarAdvertencia("Sembraste");
            return;
        }

        GameObject prefab = null;
        switch (tipoAsembrar)
        {
            case Planta.TipoPlanta.Cerezas:
                prefab = prefabCerezas;
                break;
            case Planta.TipoPlanta.Bananos:
                prefab = prefabBananos;
                break;
            case Planta.TipoPlanta.Manzanas:
                prefab = prefabManzanas;
                break;
        }

        if (prefab != null)
        {
            if (AudioManager.instance != null && AudioManager.instance.sembrar != null)
            {
                AudioManager.instance.ReproducirSonido(AudioManager.instance.sembrar);
            }
            Instantiate(prefab, puntoSiembra.position, Quaternion.identity);

            UIManagerMensajes.instance.MostrarAdvertencia($"Sembraste: {tipoAsembrar}");

            yaSembrado = true;
            VolverASitio();
        }
    }

    private void VolverASitio()
    {
        ObjetoLlevable llevable = GetComponent<ObjetoLlevable>();
        if (llevable != null)
        {
            llevable.Soltar();
        }

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            Animator anim = jugador.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("Interactuando", false);
            }
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        StartCoroutine(RecolocarDespuesDeSoltar());
    }

    private IEnumerator RecolocarDespuesDeSoltar()
    {
        yield return new WaitForSeconds(0.1f);

        transform.SetParent(padreInicial);
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        yaSembrado = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaSiembra") && other.gameObject.activeSelf)
        {
            enZonaSiembra = true;
            puntoSiembra = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZonaSiembra"))
        {
            enZonaSiembra = false;
            puntoSiembra = null;
        }
    }
}
