using UnityEngine;

public class InteraccionJugador : MonoBehaviour
{
    [Header("UI")]
     // Texto que aparece cuando hay algo para interactuar

    [Header("Detección de Interacción (OverlapBox)")]
    public Transform centroDeteccion;              // Punto desde donde se lanza la detección de objetos cercanos
    public Vector3 tamanoDeteccion = new Vector3(1f, 1f, 1f); // Tamaño del área para detectar objetos interactuables
    public LayerMask capaInteraccion;              // Capa que define qué objetos pueden ser interactuados

    public Transform puntoCarga;                    // Empty donde se colocan objetos que se llevan

    private IInteractuable interactuableActual;    // Referencia al objeto con el que se puede interactuar actualmente
    private Animator animator;                      // Referencia al Animator para controlar animaciones

    private void Start()
    {
        animator = GetComponent<Animator>();       // Obtiene el Animator asociado a este GameObject
    }

    void Update()
    {
        interactuableActual?.xD();

        DetectarObjetoCercano();                     // Detecta objetos interactuables en el área

        // Interacción con tecla E
        if (Input.GetKeyDown(KeyCode.E) && interactuableActual != null)
        {
            interactuableActual.Interactuar(gameObject);  // Llama al método Interactuar del objeto detectado

            // Alterna la animación "Interactuando" como bool
            bool interactuando = animator.GetBool("Interactuando");
            animator.SetBool("Interactuando", !interactuando);
        }

        // Interacción con click izquierdo con raycast

        if (Input.GetMouseButtonDown(0) && interactuableActual != null)
        {
            interactuableActual.InteractuarClick(gameObject);
        }
    }

    void DetectarObjetoCercano()
    {
        interactuableActual = null; // Resetea la referencia cada frame

        // Busca colisionadores dentro del área definida con OverlapBox, solo en la capa especificada
        Collider[] colisiones = Physics.OverlapBox(
            centroDeteccion.position,     // Centro de la caja de detección
            tamanoDeteccion / 2f,         // Mitad del tamaño (porque OverlapBox usa half extents)
            Quaternion.identity,          // Sin rotación en la caja de detección
            capaInteraccion               // Solo objetos en la capa de interacción
        );

        // Recorre cada collider encontrado
        foreach (Collider col in colisiones)
        {
            // Si el objeto tiene un componente que implemente IInteractuable, se guarda la referencia
            if (col.TryGetComponent<IInteractuable>(out interactuableActual))
            {
                Debug.Log(col.gameObject.name);
                /*textoInteractuar?.SetActive(true); */ // Activa el texto de interacción
                return;                             // Sale del método ya que encontró un objeto
            }
        }

        // Si no encontró nada interactuable, oculta el texto de interacción
        //textoInteractuar?.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (centroDeteccion == null) return;

        Gizmos.color = Color.cyan;                     // Color cyan para el gizmo
        Gizmos.DrawWireCube(centroDeteccion.position, tamanoDeteccion);  // Dibuja la caja de detección en la escena
    }
}
