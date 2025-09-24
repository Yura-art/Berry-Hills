using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CajaPlantas : MonoBehaviour, IInteractuableF
{
    [Header("Capacidad")]
    public int capacidadMaxima = 5;

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI capacidadText;

    [Header("Tipo de planta que acepta esta caja")]
    public Planta.TipoPlanta tipoAceptado;

    private List<Planta> plantasGuardadas = new List<Planta>();

    public bool EstaLlena
    {
        get { return plantasGuardadas.Count >= capacidadMaxima; }
    }

    public CajasManager gestor;

    private void Start()
    {
        texto();
    }

    private void Update()
    {
        texto();
    }

    public void InteractuarClick(GameObject interactor) // ✅ solo F
    {
        Planta planta = interactor.GetComponentInChildren<Planta>();

        if (planta != null)
        {
            if (planta.tipoActual != tipoAceptado)
            {
                UIManagerMensajes.instance.MostrarAdvertencia($"Esta caja solo acepta plantas del tipo {tipoAceptado}.");
                return;
            }

            if (!EstaLlena)
            {
                plantasGuardadas.Add(planta);
                planta.gameObject.SetActive(false);
                UIManagerMensajes.instance.MostrarAdvertencia($"Planta guardada en la caja. Total plantas: {plantasGuardadas.Count}");

                ObjetoLlevable llevable = planta.GetComponent<ObjetoLlevable>();
                if (llevable != null)
                {
                    llevable.Soltar();
                }

                if (AudioManager.instance != null && AudioManager.instance.guardarObjeto != null)
                {
                    AudioManager.instance.ReproducirSonido(AudioManager.instance.guardarObjeto);
                }

                Animator anim = interactor.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool("Interactuando", false);
                }

                if (EstaLlena && gestor != null)
                {
                    gestor.VerificarCajasLlenas();
                }
            }
            else
            {
                Debug.Log("La caja está llena.");
            }
        }
        else
        {
            Debug.Log("No llevas ninguna planta para guardar.");
        }
    }

    public List<Planta> ObtenerPlantasGuardadas()
    {
        return plantasGuardadas;
    }

    public void texto()
    {
        if (capacidadText != null)
        {
            capacidadText.text = plantasGuardadas.Count + " / " + capacidadMaxima;
        }
    }

    public void ReiniciarCaja()
    {
        plantasGuardadas.Clear();
        texto();
    }

    public void xD()
    {
        Debug.Log("Caja Plantas");
    }
}
