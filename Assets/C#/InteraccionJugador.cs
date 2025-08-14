using UnityEngine;

public class InteraccionJugador : MonoBehaviour
{
    [Header("UI")]
     // Texto que aparece cuando hay algo para interactuar

    [Header("Detecci�n de Interacci�n (OverlapBox)")]
    public Transform centroDeteccion;              // Punto desde donde se lanza la detecci�n de objetos cercanos
    public Vector3 tamanoDeteccion = new Vector3(1f, 1f, 1f); // Tama�o del �rea para detectar objetos interactuables
    public LayerMask capaInteraccion;              // Capa que define qu� objetos pueden ser interactuados

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

        DetectarObjetoCercano();                     // Detecta objetos interactuables en el �rea

        // Interacci�n con tecla E
        if (Input.GetKeyDown(KeyCode.E) && interactuableActual != null)
        {
            interactuableActual.Interactuar(gameObject);  // Llama al m�todo Interactuar del objeto detectado

            // Alterna la animaci�n "Interactuando" como bool
            bool interactuando = animator.GetBool("Interactuando");
            animator.SetBool("Interactuando", !interactuando);
        }

        // Interacci�n con click izquierdo con raycast

        if (Input.GetMouseButtonDown(0) && interactuableActual != null)
        {
            interactuableActual.InteractuarClick(gameObject);
        }
    }

    void DetectarObjetoCercano()
    {
        interactuableActual = null; // Resetea la referencia cada frame

        // Busca colisionadores dentro del �rea definida con OverlapBox, solo en la capa especificada
        Collider[] colisiones = Physics.OverlapBox(
            centroDeteccion.position,     // Centro de la caja de detecci�n
            tamanoDeteccion / 2f,         // Mitad del tama�o (porque OverlapBox usa half extents)
            Quaternion.identity,          // Sin rotaci�n en la caja de detecci�n
            capaInteraccion               // Solo objetos en la capa de interacci�n
        );

        // Recorre cada collider encontrado
        foreach (Collider col in colisiones)
        {
            // Si el objeto tiene un componente que implemente IInteractuable, se guarda la referencia
            if (col.TryGetComponent<IInteractuable>(out interactuableActual))
            {
                Debug.Log(col.gameObject.name);
                /*textoInteractuar?.SetActive(true); */ // Activa el texto de interacci�n
                return;                             // Sale del m�todo ya que encontr� un objeto
            }
        }

        // Si no encontr� nada interactuable, oculta el texto de interacci�n
        //textoInteractuar?.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (centroDeteccion == null) return;

        Gizmos.color = Color.cyan;                     // Color cyan para el gizmo
        Gizmos.DrawWireCube(centroDeteccion.position, tamanoDeteccion);  // Dibuja la caja de detecci�n en la escena
    }
}
