using UnityEngine;

public class Regadera : ObjetoLlevable
{
    public float cantidadAgua = 10f;

    public override void xD()
    {
    }

    // ✅ No necesita InteractuarClick porque solo funciona con plantas
    public void Regar(CrecimientoPlanta planta)
    {
        if (cantidadAgua > 0)
        {
            planta.RecibirAgua(1f);
            cantidadAgua -= 1f;

            if (AudioManager.instance != null && AudioManager.instance.regar != null)
            {
                AudioManager.instance.ReproducirSonido(AudioManager.instance.regar);
            }
        }
        else
        {
            Debug.Log("La regadera está vacía.");
        }
    }
}
