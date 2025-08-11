using UnityEngine;
using TMPro;

public class MostrarTiempo : MonoBehaviour
{
    public ControlTiempo controladorTiempo;
    public TMP_Text textoTiempo; 

    void Update()
    {
        if (controladorTiempo != null && textoTiempo != null)
        {
            float tiempo = controladorTiempo.GetTiempoRestante();
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempo).ToString();
        }
    }
}
