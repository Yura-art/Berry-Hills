using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CajaPlantas : MonoBehaviour, IInteractuable
{
    [Header("Capacidad")]
    public int capacidadMaxima = 5;

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI capacidadText;

    [Header("Tipo de planta que acepta esta caja")]
    public Planta.TipoPlanta tipoAceptado;

    private List<Planta> plantasGuardadas = new List<Planta>();

    // ✅ Propiedad para saber si la caja está llena
    public bool EstaLlena
    {
        get { return plantasGuardadas.Count >= capacidadMaxima; }
    }

    // ✅ Referencia al gestor para avisar cuando se llene
    public GestorCajas gestor;

    private void Start()
    {
        texto();
    }

    private void Update()
    {
        texto();
    }

    public void Interactuar(GameObject interactor)
    {
        Debug.Log("Interactuaste con la caja.");
    }

    public void InteractuarClick(GameObject interactor)
    {
        Planta planta = interactor.GetComponentInChildren<Planta>();

        if (planta != null)
        {
            if (planta.tipoActual != tipoAceptado)
            {
                Debug.Log($"Esta caja solo acepta plantas del tipo {tipoAceptado}.");
                return;
            }

            if (!EstaLlena)
            {
                plantasGuardadas.Add(planta);
                planta.gameObject.SetActive(false);
                Debug.Log($"Planta guardada en la caja. Total plantas: {plantasGuardadas.Count}");

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

                // ✅ Si ahora la caja está llena, avisamos al gestor
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
        plantasGuardadas.Clear(); // Elimina todas las plantas guardadas
        texto(); // Actualiza el UI de capacidad
    }


    public void xD()
    {
        Debug.Log("Caja Plantas");
    }
}
