using UnityEngine;

public class Regadera : ObjetoLlevable
{
    public float cantidadAgua = 10f;  // Cantidad inicial de agua en la regadera

    public override void InteractuarClick(GameObject interactor)
    {
    }

    // M�todo para regar una planta
    public void Regar(CrecimientoPlanta planta)
    {
        // Si a�n queda agua disponible
        if (cantidadAgua > 0)
        {
            planta.RecibirAgua(1f);  // Llama al m�todo de la planta para recibir 1 unidad de agua
            cantidadAgua -= 1f;      // Reduce la cantidad de agua en la regadera
        }
        else
        {
            Debug.Log("La regadera est� vac�a."); // Mensaje si no queda agua para regar
        }
    }

    public override void xD()
    {
    }
}
