using UnityEngine;

public class Regadera : ObjetoLlevable
{
    public float cantidadAgua = 10f;  // Cantidad inicial de agua en la regadera

    public override void InteractuarClick(GameObject interactor)
    {
    }

    // Método para regar una planta
    public void Regar(CrecimientoPlanta planta)
    {
        // Si aún queda agua disponible
        if (cantidadAgua > 0)
        {
            planta.RecibirAgua(1f);  // Llama al método de la planta para recibir 1 unidad de agua
            cantidadAgua -= 1f;      // Reduce la cantidad de agua en la regadera

            if (AudioManager.instance != null && AudioManager.instance.regar != null)
            {
                AudioManager.instance.ReproducirSonido(AudioManager.instance.regar);
            }
        }
        else
        {
            Debug.Log("La regadera está vacía."); // Mensaje si no queda agua para regar
        }
    }

    public override void xD()
    {
    }
}
