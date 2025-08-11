using UnityEngine;

public class RotacionInfinitaSegura : MonoBehaviour
{
    [Header("Referencia al objeto que se va a rotar")]
    public Transform objetoVisual; // Arrastra aqu� el modelo o mesh en el inspector

    [Header("Velocidad de rotaci�n")]
    public Vector3 velocidadRotacion = new Vector3(0f, 50f, 0f);

    void Update()
    {
        if (objetoVisual != null)
        {
            // Rotamos SOLO el mesh visual, sin afectar la ra�z (evita deformaciones)
            objetoVisual.Rotate(velocidadRotacion * Time.deltaTime, Space.Self);
        }
    }
}
