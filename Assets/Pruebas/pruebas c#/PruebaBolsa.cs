using UnityEngine;
using System.Collections;

public class PruebaBolsa : ObjetoInteractuable
{
    [Header("Tipo de planta a sembrar")]
    public Planta.TipoPlanta tipoAsembrar;

    [Header("Prefabs de plantas")]
    public GameObject prefabCerezas;
    public GameObject prefabBananos;
    public GameObject prefabManzanas;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Transform padreInicial;

    private void Start()
    {
        // Guardamos la posición original
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        padreInicial = transform.parent;
    }

    public GameObject ObtenerPrefab()
    {
        switch (tipoAsembrar)
        {
            case Planta.TipoPlanta.Cerezas: return prefabCerezas;
            case Planta.TipoPlanta.Bananos: return prefabBananos;
            case Planta.TipoPlanta.Manzanas: return prefabManzanas;
            default: return null;
        }
    }

    public override void Usar()
    {
        UIInventario.Instance.MostrarMensaje("Necesitas estar en una zona de siembra");
    }

    //Método para devolver la bolsa a su lugar
    public void VolverASitio()
    {
        StartCoroutine(RecolocarDespuesDeSoltar());
    }

    private IEnumerator RecolocarDespuesDeSoltar()
    {
        yield return new WaitForSeconds(0.1f);

        transform.SetParent(padreInicial);
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        gameObject.SetActive(true);
        UIInventario.Instance.MostrarMensaje("La bolsa volvió a su lugar");
        AudioManager.instance.ReproducirSonido(AudioManager.instance.sembrar);
    }
}
