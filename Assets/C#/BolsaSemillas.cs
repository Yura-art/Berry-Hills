using UnityEngine;
using System.Collections;


public class BolsaSemillas : ObjetoLlevable
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

    // 🔹 Posición original para resetear la bolsa
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Transform padreInicial;

    private void Start()
    {
        // Guardar la posición, rotación y padre original al iniciar
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
        base.Interactuar(jugador);
        // Ya no destruimos la bolsa aquí
    }

    public override void InteractuarClick(GameObject interactor)
    {
        Debug.Log("interactua");
        Sembrar();
    }

    public void Sembrar()
    {
        if (!enZonaSiembra || puntoSiembra == null)
        {
            Debug.Log("No estás en una zona de siembra.");
            return;
        }

        if (yaSembrado)
        {
            Debug.Log("Ya sembraste, pero se reiniciará.");
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
            Debug.Log($"Sembraste: {tipoAsembrar}");

            // Marcar como sembrado
            yaSembrado = true;

            // 🔹 Reiniciar posición de la bolsa
            VolverASitio();
        }
    }

    private void VolverASitio()
    {
        // 1️⃣ Soltar usando tu sistema
        ObjetoLlevable llevable = GetComponent<ObjetoLlevable>();
        if (llevable != null)
        {
            llevable.Soltar();
        }

        // 🔹 Apagar animación de llevar
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            Animator anim = jugador.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("Interactuando", false); // o el nombre que uses para "cargando"
            }
        }

        // 2️⃣ Desactivar interacción momentáneamente para evitar conflictos
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // 3️⃣ Esperar un momento antes de moverla
        StartCoroutine(RecolocarDespuesDeSoltar());
    }


    private IEnumerator RecolocarDespuesDeSoltar()
    {
        yield return new WaitForSeconds(0.1f); // Pequeño delay para evitar tirones

        transform.SetParent(padreInicial);
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        // 4️⃣ Volver a activar el collider
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        // 5️⃣ Resetear estado
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
