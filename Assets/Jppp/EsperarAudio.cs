using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EsperarAudio : MonoBehaviour
{
    [SerializeField] private AudioSource fuente;

    [Header("Eventos")]
    [Tooltip("Se dispara justo cuando el audio comienza a sonar")]
    [SerializeField] private UnityEvent alIniciar;

    [Tooltip("Se dispara cuando el audio termina por completo")]
    [SerializeField] private UnityEvent alTerminar;

    [Header("Configuración")]
    [SerializeField] private bool reproducirAlIniciar = true;

    private Coroutine corrutinaAudio;

    private void Start()
    {
        if (fuente == null)
        {
            Debug.LogWarning($"[EsperarAudio] No hay AudioSource asignado en '{gameObject.name}'");
            return;
        }

        if (reproducirAlIniciar)
        {
            ReproducirYEsperar();
        }
    }

    public void ReproducirYEsperar()
    {
        // Si ya había un audio reproduciéndose desde este script, detenemos la corrutina anterior
        if (corrutinaAudio != null)
        {
            StopCoroutine(corrutinaAudio);
        }

        corrutinaAudio = StartCoroutine(Ejecutar());
    }

    private IEnumerator Ejecutar()
    {
        Debug.Log($"[EsperarAudio] Reproduciendo '{fuente.clip?.name}'");

        // 1. Invocamos los eventos de inicio (p. ej. bloquear movimiento)
        alIniciar?.Invoke();

        // 2. Reproducimos el sonido
        fuente.Play();

        // 3. Esperamos 1 frame para asegurar que el motor de audio actualice 'isPlaying' a true
        yield return null;

        // 4. Esperamos mientras el audio se siga reproduciendo
        yield return new WaitWhile(() => fuente.isPlaying);

        Debug.Log($"[EsperarAudio] Terminó '{fuente.clip?.name}' - invocando eventos finales");

        // 5. Invocamos los eventos de finalización (p. ej. desbloquear movimiento)
        alTerminar?.Invoke();

        corrutinaAudio = null;
    }
}