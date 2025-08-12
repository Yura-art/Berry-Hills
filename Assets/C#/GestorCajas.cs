using UnityEngine;

public class GestorCajas : MonoBehaviour
{

    [Header ("Cajas")]
    public CajaPlantas caja1;
    public CajaPlantas caja2;
    public CajaPlantas caja3;
    public CajaPlantas caja4;
    public CajaPlantas caja5;
    public CajaPlantas caja6;

    [Header("Frutas")]
    public GameObject bananos;
    public GameObject cerezas;

    [Header ("Personajes")]
    public GameObject oveja;
    public GameObject lobo;

    [Header("Camaras")]
    public GameObject camara1;
    public GameObject camara2;

    public GameObject Ganaste;

    private void Start()
    {
        // ✅ Asignamos este gestor a cada caja
        caja1.gestor = this;
        caja2.gestor = this;
        caja3.gestor = this;
        caja4.gestor = this;
        caja5.gestor = this;
        caja6.gestor = this;

        oveja.SetActive(true);
        lobo.SetActive(false);

        camara1.SetActive(true);
        camara2.SetActive(false);

        Ganaste.SetActive(false);
    }

    // ✅ Llamado por cada caja cuando se llena
    public void VerificarCajasLlenas()
    {
        // Fase 1 → Bananos
        if (caja1.EstaLlena && !bananos.activeSelf)
        {
            ActivarBananos();
            ReiniciarTodasLasCajas();
            return; // Salimos para evitar que se chequee lo siguiente
        }

        // Fase 2 → Cerezas
        if (caja1.EstaLlena && caja2.EstaLlena && !cerezas.activeSelf)
        {
            ActivarCerezas();
            ReiniciarTodasLasCajas();
            return;
        }

        // Fase 3 → Cambio de personajes y cámaras
        if (caja1.EstaLlena && caja2.EstaLlena && caja3.EstaLlena)
        {
            PasarASiguienteFase();
            ReiniciarTodasLasCajas();
        }
    }


    private void PasarASiguienteFase()
    {
        oveja.SetActive(false);
        lobo.SetActive(true);

        camara1.SetActive(false);
        camara2.SetActive(true);

        if (caja4.EstaLlena && lobo)
        {
            Time.timeScale = 0f;
            Ganaste.SetActive(true);
        }
    }

    void ActivarBananos()
    {
        bananos.SetActive(true);
        Debug.Log("✅ Bananos desbloqueados");
    }

    void ActivarCerezas()
    {
        cerezas.SetActive(true);
        Debug.Log("✅ Cerezas desbloqueadas");
    }

    public void ReiniciarTodasLasCajas()
    {
        caja1.ReiniciarCaja();
        caja2.ReiniciarCaja();
        caja3.ReiniciarCaja();
        caja4.ReiniciarCaja();
        caja5.ReiniciarCaja();
        caja6.ReiniciarCaja();
    }

}
