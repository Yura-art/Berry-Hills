using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventario : MonoBehaviour
{
    public static UIInventario Instance;

    public TextMeshProUGUI mensajeUI; //Texto para mensajes al jugador

    private void Awake()
    {
        Instance = this;
    }

    //Muestra un mensaje en pantalla
    public void MostrarMensaje(string mensaje)
    {
        if (mensajeUI != null) mensajeUI.text = mensaje;
    }
}
