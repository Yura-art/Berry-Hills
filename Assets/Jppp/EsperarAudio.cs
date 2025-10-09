using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EsperarAudio : MonoBehaviour
{
    [SerializeField] AudioSource fuente;
    [SerializeField] UnityEvent alTerminar;
    [SerializeField] bool reproducirAlIniciar = true;

    private void Start()
    {
        if (fuente == null)
        {
            Debug.LogWarning($"[EsperarAudio] No hay AudioSource asignado en '{gameObject.name}'");
            return;
        }

        if (reproducirAlIniciar)
            ReproducirYEsperar();
    }

    public void ReproducirYEsperar()
    {
        StartCoroutine(Ejecutar());
    }

    private IEnumerator Ejecutar()
    {
        Debug.Log($"[EsperarAudio] Reproduciendo '{fuente.clip?.name}'");
        fuente.Play();
        // Espera mientras se esté reproduciendo (más fiable que clip.length)
        yield return new WaitWhile(() => fuente.isPlaying);
        Debug.Log($"[EsperarAudio] Terminó '{fuente.clip?.name}' - invocando eventos");
        alTerminar?.Invoke();
    }
}

