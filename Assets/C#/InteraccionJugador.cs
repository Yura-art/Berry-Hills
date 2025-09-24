using UnityEngine;

public class InteraccionJugador : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject presionaE;
    [SerializeField] GameObject presionaF;

    [Header("Detección de Interacción (OverlapBox)")]
    public Transform centroDeteccion;
    public Vector3 tamanoDeteccion = new Vector3(1f, 1f, 1f);
    public LayerMask capaInteraccion;

    public Transform puntoCarga;

    private IInteractuableE interactuableE;
    private IInteractuableF interactuableF;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        presionaE.SetActive(false);
        presionaF.SetActive(false);
    }

    void Update()
    {
        DetectarObjetoCercano();

        if (Input.GetKeyDown(KeyCode.E) && interactuableE != null)
        {
            interactuableE.Interactuar(gameObject);
            animator.SetBool("Interactuando", true);
        }

        if (Input.GetKeyDown(KeyCode.F) && interactuableF != null)
        {
            interactuableF.InteractuarClick(gameObject);
            presionaE.SetActive(false);
            presionaF.SetActive(false);
        }
    }

    void DetectarObjetoCercano()
    {
        interactuableE = null;
        interactuableF = null;

        Collider[] colisiones = Physics.OverlapBox(
            centroDeteccion.position,
            tamanoDeteccion / 2f,
            Quaternion.identity,
            capaInteraccion
        );

        foreach (Collider col in colisiones)
        {
            if (col.TryGetComponent<IInteractuableE>(out interactuableE))
                presionaE?.SetActive(true);

            if (col.TryGetComponent<IInteractuableF>(out interactuableF))
                presionaF?.SetActive(true);

            if (interactuableE != null || interactuableF != null)
                return;
        }

        presionaE?.SetActive(false);
        presionaF?.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (centroDeteccion == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(centroDeteccion.position, tamanoDeteccion);
    }
}
