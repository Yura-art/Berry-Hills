using UnityEngine;

public class GestorCajas : MonoBehaviour
{
    public CajaPlantas caja1;
    public CajaPlantas caja2;
    public CajaPlantas caja3;
    public CajaPlantas caja4;

    public GameObject oveja;
    public GameObject lobo;

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

        oveja.SetActive(true);
        lobo.SetActive(false);

        camara1.SetActive(true);
        camara2.SetActive(false);

        Ganaste.SetActive(false);
    }

    // ✅ Llamado por cada caja cuando se llena
    public void VerificarCajasLlenas()
    {
        if (caja1.EstaLlena && caja2.EstaLlena && caja3.EstaLlena)
        {
            Debug.Log("✅ Todas las cajas están llenas. Pasando a la siguiente fase...");
            PasarASiguienteFase();
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
}
