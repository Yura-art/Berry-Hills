using System;
using UnityEngine;

public abstract class ObjetoLlevable : MonoBehaviour, IInteractuable
{
    private Rigidbody rb;
    private Collider col;
    public bool siendoLlevado = false;
    private Transform puntoCarga;
    private GameObject jugadorQueLleva;

    protected bool puedeCargar = true;

    public abstract void xD();
    public virtual void Update()
    {
        Invoke("VerificarRbCol", 0.2f);
    }

    public virtual void Interactuar(GameObject interactor)
    {

        if (AudioManager.instance != null && AudioManager.instance.tomarObjeto != null)
        {
            AudioManager.instance.ReproducirSonido(AudioManager.instance.tomarObjeto);
        }
        if (!siendoLlevado)
        {
            // Recoger el objeto
            puntoCarga = interactor.transform.Find("Deteccion Interactuar");
            if (puntoCarga == null)
            {
                Debug.LogWarning("El jugador no tiene un objeto hijo llamado 'Deteccion Interactuar'.");
                return;
            }

            siendoLlevado = true;
            jugadorQueLleva = interactor;

            rb.isKinematic = true;
            col.isTrigger = true;

            transform.SetParent(puntoCarga);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            Soltar();
        }
    }

    public void Soltar()
    {
        siendoLlevado = false;
        jugadorQueLleva = null;

        transform.SetParent(null);

        rb.isKinematic = false;
        col.isTrigger = false;
    }

    void VerificarRbCol()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public abstract void InteractuarClick(GameObject interactor);
}
