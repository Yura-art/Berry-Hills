using UnityEngine;
using System.Collections;
using TMPro;

public class FuenteAgua : MonoBehaviour, IInteractuableF
{
    [Header("Recarga de Agua")]
    public float cantidadRecarga = 10f;
    public float tiempoRecarga = 5f;

    [Header("UI")]
    public TextMeshProUGUI textoCooldown;

    private bool enCooldown = false;

    public void InteractuarClick(GameObject interactor) // ✅ solo F
    {
        if (enCooldown)
        {
            Debug.Log("La fuente está recargando, espera...");
            return;
        }

        Regadera regadera = interactor.GetComponentInChildren<Regadera>();
        if (regadera != null)
        {
            regadera.cantidadAgua += cantidadRecarga;
            UIManagerMensajes.instance.MostrarAdvertencia($"Regadera recargada. Agua actual: {regadera.cantidadAgua}");

            if (AudioManager.instance != null)
            {
                AudioManager.instance.ReproducirFuenteAgua();
            }

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

        MovimientoJugador mov = FindObjectOfType<MovimientoJugador>();
        if (mov != null) mov.puedeMover = false;

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

        if (AudioManager.instance != null)
        {
            AudioManager.instance.DetenerFuenteAgua();
        }

        if (mov != null) mov.puedeMover = true;

        enCooldown = false;
    }
}
