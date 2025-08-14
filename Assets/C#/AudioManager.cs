using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicaFondo;    // M�sica de fondo en loop
    public AudioSource efectos;        // Efectos puntuales (PlayOneShot)
    public AudioSource caminar;      // Pasos en loop (Play / Stop)
    public AudioSource correr;

    [Header("Clips")]
    public AudioClip tomarObjeto;
    public AudioClip regar;
    //public AudioClip recolectarAgua;
    public AudioClip cosechar;
    public AudioClip guardarObjeto;
    public AudioClip sembrar;

    [Header("Audio Sources especiales")]
    public AudioSource fuenteAgua;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }
    void Update()
    {
        if (musicaFondo != null && !musicaFondo.isPlaying)
        {
            musicaFondo.Play();
        }
    }


    void Start()
    {
        // Iniciar m�sica si no est� sonando
        if (musicaFondo != null && !musicaFondo.isPlaying)
        {
            musicaFondo.loop = true;
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

    //Jp estuvo aqui jsjs para �rreglar el sonido en loop de la recarga
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
}
