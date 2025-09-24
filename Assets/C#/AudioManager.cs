using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicaFondo;    // Música de fondo en loop
    public AudioSource efectos;        // Efectos puntuales (PlayOneShot)
    public AudioSource caminar;        // Pasos en loop (Play / Stop)
    public AudioSource correr;
    public AudioSource boton;          // Para sonidos de botones
    public AudioSource Llamada;
    public AudioSource dialogo;        // 🔊 Canal dedicado solo para diálogos

    [Header("Clips")]
    public AudioClip tomarObjeto;
    public AudioClip regar;
    public AudioClip cosechar;
    public AudioClip guardarObjeto;
    public AudioClip sembrar;
    public AudioClip peridico;

    [Header("UI Clips")]
    public AudioClip botonClick;
    public AudioClip botonHover;
    public AudioClip sonidoDialogo;
    public AudioClip sonidoLlamada;

    [Header("Audio Sources especiales")]
    public AudioSource fuenteAgua;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicaFondo != null)
        {
            musicaFondo.Stop();
            musicaFondo.time = 0f; // arranca desde el inicio
            musicaFondo.loop = true;
            musicaFondo.Play();
        }
    }

    void Update()
    {
        if (musicaFondo != null && !musicaFondo.isPlaying)
        {
            musicaFondo.Play();
        }
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null && efectos != null)
        {
            efectos.PlayOneShot(clip);
        }
    }

    public void ReproducirFuenteAgua()
    {
        if (fuenteAgua != null && !fuenteAgua.isPlaying)
            fuenteAgua.Play();
    }

    public void DetenerFuenteAgua()
    {
        if (fuenteAgua != null && fuenteAgua.isPlaying)
            fuenteAgua.Stop();
    }

    public void ReproducirBotonClick()
    {
        if (boton != null && botonClick != null)
        {
            boton.clip = botonClick;
            boton.Play();
        }
    }

    public void ReproducirBotonHover()
    {
        if (boton != null && botonHover != null)
        {
            boton.clip = botonHover;
            boton.Play();
        }
    }

    // 🎙️ Diálogo (controlable)
    public void ReproducirDialogo()
    {
        if (dialogo != null && sonidoDialogo != null)
        {
            dialogo.clip = sonidoDialogo;
            dialogo.Play();
        }
    }

    public void DetenerDialogo()
    {
        if (dialogo != null && dialogo.isPlaying)
        {
            dialogo.Stop();
        }
    }

    public void ReproducirLlamada()
    {
        if (sonidoLlamada != null)
        {
            Llamada.clip = sonidoLlamada;
            Llamada.Play();
        }
    }

    public void CerrarLlamada()
    {
        if (Llamada != null && Llamada.isPlaying)
        {
            Llamada.Stop();
        }
    }
}
