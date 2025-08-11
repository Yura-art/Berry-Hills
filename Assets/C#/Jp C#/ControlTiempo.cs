using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTiempo : MonoBehaviour
{
    public float tiempoMaximo = 60f; // Tiempo inicial de la fase
    private float tiempoRestante;

    private int faseActual = 1;

    private bool juegoTerminado = false;

    void Start()
    {
        tiempoRestante = tiempoMaximo;
    }

    void Update()
    {
        if (juegoTerminado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.G)) 
            {
                Ganar();
            }

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                Perder();
            }
        }
    }

    // Llama esta función cuando el jugador realiza la tarea correctamente
    public void Ganar()
    {
        juegoTerminado = true;
        int puntaje = Mathf.CeilToInt(tiempoRestante);
        Debug.Log("¡Ganaste! Puntaje: " + puntaje);

        // Preparar siguiente fase
        SiguienteFase();
    }

    void Perder()
    {
        juegoTerminado = true;
        Debug.Log("Se acabó el tiempo. Perdiste.");

        // Reiniciar o terminar el juego
        ReiniciarFase();
    }

    void SiguienteFase()
    {
        faseActual++;

        // Puedes cambiar el tiempo para la siguiente fase como quieras
        // Por ejemplo, disminuir tiempo cada fase
        tiempoMaximo = Mathf.Max(10f, tiempoMaximo - 5f);

        ReiniciarFase();
    }

    void ReiniciarFase()
    {
        tiempoRestante = tiempoMaximo;
        juegoTerminado = false;

        // Aquí podrías cargar una nueva escena o resetear elementos para la nueva fase
        // Por ejemplo, si quieres cambiar de escena:
        // SceneManager.LoadScene("NombreDeLaEscenaDeLaFase" + faseActual);

        Debug.Log("Fase " + faseActual + " iniciada con tiempo: " + tiempoMaximo);
    }

    // Opcional: método para obtener tiempo restante o puntaje actual
    public float GetTiempoRestante()
    {
        return tiempoRestante;
    }
}
