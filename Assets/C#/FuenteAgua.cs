using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FuenteAgua : MonoBehaviour, IInteractuable
{
    [Header("Recarga de Agua")]
    public float cantidadRecarga = 10f; // Cuánta agua añade a la regadera
    public float tiempoRecarga = 5f;    // Segundos que tarda en poder usarse otra vez

    [Header("UI")]
    public TextMeshProUGUI textoCooldown;          // Texto para mostrar el tiempo restante (asignar en Inspector)

    private bool enCooldown = false;

    public void Interactuar(GameObject interactor)
    {
        // No usamos interacción por tecla en este caso
    }

    public void InteractuarClick(GameObject interactor)
    {
        if (enCooldown)
        {
            Debug.Log("La fuente está recargando, espera...");
            return;
        }

        // Buscar la regadera que el jugador lleva en la mano
        Regadera regadera = interactor.GetComponentInChildren<Regadera>();
        if (regadera != null)
        {
            regadera.cantidadAgua += cantidadRecarga;
            Debug.Log($"Regadera recargada. Agua actual: {regadera.cantidadAgua}");

            //Jp estuvo aqui jsjs para árreglar el sonido en loop de la recarga
            if (AudioManager.instance != null)
            {
                AudioManager.instance.ReproducirFuenteAgua();
            }

            // Inicia el cooldown
            StartCoroutine(CooldownRecarga());
        }
        else
        {
            Debug.Log("No llevas una regadera para recargar.");
        }
    }

    public void xD()
    {
        Debug.Log("Fuente Agua");
    }

    private IEnumerator CooldownRecarga()
    {
        enCooldown = true;
        float tiempoRestante = tiempoRecarga;

        if (textoCooldown != null)
            textoCooldown.gameObject.SetActive(true);

        while (tiempoRestante > 0)
        {
            if (textoCooldown != null)
                textoCooldown.text = $"Recargando: {tiempoRestante:F1}s";

            yield return new WaitForSeconds(0.1f);
            tiempoRestante -= 0.1f;
        }

        if (textoCooldown != null)
            textoCooldown.gameObject.SetActive(false);

        //Jp estuvo aqui jsjs para árreglar el sonido en loop de la recarga
        if (AudioManager.instance != null)
        {
            AudioManager.instance.DetenerFuenteAgua();
        }

        enCooldown = false;
    }
}
