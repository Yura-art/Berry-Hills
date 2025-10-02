using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FasePrueba
{
    public string nombre;
    public List<GameObject> activar;
    public List<GameObject> desactivar;
    public int cajasNecesarias = 1;
    public bool esFaseFinal = false;
}

public class ManagerCajaPrueba : MonoBehaviour
{
    [Header("Cajas")]
    public List<ZonaCaja> cajas = new List<ZonaCaja>();

    [Header("Fases del juego")]
    public List<FasePrueba> fases = new List<FasePrueba>(); // 🔑 Usamos FasePrueba

    [Header("Tiempo")]
    public ControlTiempo controlTiempo;

    [Header("UI")]
    public GameObject panelGanaste;

    [Header("Audio")]
    public AudioSource musicaFondo;

    private int faseActual = 0;

    private void Start()
    {
        // Asignar gestor a cada caja
        foreach (var caja in cajas)
        {
            caja.gestor = this; // asegúrate de que ZonaCaja tiene 'public ManagerCajaPrueba gestor;'
        }

        panelGanaste.SetActive(false);

        if (controlTiempo != null)
            controlTiempo.IniciarTiempo();
    }

    public void VerificarCajasLlenas()
    {
        if (faseActual >= fases.Count) return;

        int cajasLlenas = 0;
        foreach (var caja in cajas)
        {
            if (caja.EstaLlena) cajasLlenas++;
        }

        if (cajasLlenas >= fases[faseActual].cajasNecesarias)
        {
            EjecutarFase(fases[faseActual]);

            if (fases[faseActual].esFaseFinal)
                return;

            faseActual++;
            controlTiempo.Ganar();
            ReiniciarTodasLasCajas();
            controlTiempo.IniciarTiempo();
        }
    }

    private void EjecutarFase(FasePrueba fase)
    {
        Debug.Log($"▶ Ejecutando fase: {fase.nombre}");

        foreach (var obj in fase.activar)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in fase.desactivar)
            if (obj != null) obj.SetActive(false);

        if (fase.esFaseFinal)
        {
            Time.timeScale = 0f;
            panelGanaste.SetActive(true);
            controlTiempo.DetenerTiempo();

            if (musicaFondo != null && musicaFondo.isPlaying)
                musicaFondo.Stop();
        }
    }

    public void ReiniciarTodasLasCajas()
    {
        foreach (var caja in cajas)
            caja.ReiniciarCaja();
    }
}
