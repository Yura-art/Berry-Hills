using UnityEngine;

public class Llamada : MonoBehaviour
{
    public AudioClip sonidoLlamada;
    private bool enLlamadaEntrante = false;

    private float volumenOriginal; // Para guardar el volumen de la música
    private float volumenOriginal2; // Para guardar el volumen de la música

    void Start()
    {
        IniciarLlamada();
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
            AudioManager.instance.musicaFondo.volume = 0.2f; // volumen reducido
        }

        // Reproducir sonido de llamada
        AudioManager.instance.ReproducirSonido(sonidoLlamada);
        volumenOriginal2 = AudioManager.instance.efectos.volume;
        AudioManager.instance.efectos.volume = 0.2f;
        AudioManager.instance.efectos.loop = true;

        UIManagerMensajes.instance.MostrarMensaje("Llamada entrante. Presiona [Q] para contestar");
    }

    void ContestarLlamada()
    {
        enLlamadaEntrante = false;

        // Detener sonido de llamada
        AudioManager.instance.efectos.Stop();
        AudioManager.instance.efectos.loop = false;
        AudioManager.instance.efectos.volume = volumenOriginal2;


        // Restaurar volumen original
        if (AudioManager.instance.musicaFondo != null)
        {
            AudioManager.instance.musicaFondo.volume = volumenOriginal;
        }

        UIManagerMensajes.instance.MostrarMensaje("Llamada contestada");

        // Iniciar diálogo
        FindObjectOfType<DialogoManager>().IniciarDialogo();
    }
}
