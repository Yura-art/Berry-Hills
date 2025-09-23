using UnityEngine;

public class Llamada : MonoBehaviour
{
    private bool enLlamadaEntrante = false;

    private float volumenOriginal; // Para guardar el volumen de la música

    public float tiempoParaIniciar = 5f;

    void Start()
    {
        Invoke(nameof(IniciarLlamada), tiempoParaIniciar);
    }

    void Update()
    {
        if (enLlamadaEntrante && Input.GetKeyDown(KeyCode.Q))
        {
            ContestarLlamada();
        }
    }

    public void IniciarLlamada()
    {
        enLlamadaEntrante = true;

        // Guardamos el volumen original y lo bajamos
        if (AudioManager.instance.musicaFondo != null)
        {
            volumenOriginal = AudioManager.instance.musicaFondo.volume;
            AudioManager.instance.musicaFondo.volume = 0.05f; // volumen reducido
        }

        // Reproducir sonido de llamada
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReproducirLlamada();
        }
        UIManagerMensajes.instance.MostrarMensaje("Llamada entrante. Presiona [Q] para contestar");
    }

    void ContestarLlamada()
    {
        enLlamadaEntrante = false;

        // Restaurar volumen original
        if (AudioManager.instance.musicaFondo != null)
        {
            AudioManager.instance.musicaFondo.volume = volumenOriginal;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.CerrarLlamada();
        }

        UIManagerMensajes.instance.MostrarMensaje("Llamada contestada");

        // Iniciar diálogo
        FindObjectOfType<DialogoManager>().IniciarDialogo();
    }
}
