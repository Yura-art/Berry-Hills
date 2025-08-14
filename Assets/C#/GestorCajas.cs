using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Fase
{
    public string nombre;
    public List<GameObject> activar;
    public List<GameObject> desactivar;
    public int cajasNecesarias = 1;
    public bool esFaseFinal = false;
}

public class GestorCajas : MonoBehaviour
{
    [Header("Cajas")]
    public List<CajaPlantas> cajas = new List<CajaPlantas>();

    [Header("Fases del juego")]
    public List<Fase> fases = new List<Fase>();

    [Header("Tiempo")]
    public ControlTiempo controlTiempo;

    [Header("UI")]
    public GameObject panelGanaste;

    [Header("Audio")]
    public AudioSource musicaFondo; // 🎵 referencia al audio de fondo

    private int faseActual = 0;

    private void Start()
    {
        // Asignar gestor a cada caja
        foreach (var caja in cajas)
        {
            caja.gestor = this;
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

        // Si se cumplen las cajas necesarias para la fase
        if (cajasLlenas >= fases[faseActual].cajasNecesarias)
        {
            EjecutarFase(fases[faseActual]);

            // Si era la última fase real (fase final marcada), no seguimos
            if (fases[faseActual].esFaseFinal)
                return;

            faseActual++;
            controlTiempo.Ganar();
            ReiniciarTodasLasCajas();
            controlTiempo.IniciarTiempo();
        }
    }

    private void EjecutarFase(Fase fase)
    {
        Debug.Log($"▶ Ejecutando fase: {fase.nombre}");

        // Activar objetos
        foreach (var obj in fase.activar)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Desactivar objetos
        foreach (var obj in fase.desactivar)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Solo mostrar Ganaste si esta fase está marcada como final
        if (fase.esFaseFinal)
        {
            Time.timeScale = 0f;
            panelGanaste.SetActive(true);
            controlTiempo.DetenerTiempo();

            // 🎵 Parar la música si está sonando
            if (musicaFondo != null && musicaFondo.isPlaying)
            {
                musicaFondo.Stop();
            }
        }
    }

    public void ReiniciarTodasLasCajas()
    {
        foreach (var caja in cajas)
        {
            caja.ReiniciarCaja();
        }
    }
}
