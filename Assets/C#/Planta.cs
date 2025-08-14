using UnityEngine;

public class Planta : MonoBehaviour
{
    public enum TipoPlanta
    {
        Cerezas,
        Bananos,
        Manzanas
    }

    public TipoPlanta tipoActual;  // El tipo que tiene esta planta
}
