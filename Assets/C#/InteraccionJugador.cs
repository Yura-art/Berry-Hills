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

    // Flags para saber si ya se interactuó con E y F
    private bool interactuoE = false;
    private bool interactuoF = false;

    private DialogoManager dialogoManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        presionaE.SetActive(false);
        presionaF.SetActive(false);

        dialogoManager = FindObjectOfType<DialogoManager>();
    }

    void Update()
    {
        DetectarObjetoCercano();

        if (Input.GetKeyDown(KeyCode.E) && interactuableE != null && !interactuoE)
        {
            interactuableE.Interactuar(gameObject);
            animator.SetBool("Interactuando", true);
            interactuoE = true;

            if (dialogoManager != null)
                dialogoManager.CumplirCondicion("interactuoE");
        }

        if (Input.GetKeyDown(KeyCode.F) && interactuableF != null && !interactuoF)
        {
            interactuableF.InteractuarClick(gameObject);
            presionaE.SetActive(false);
            presionaF.SetActive(false);
            interactuoF = true;

            if (dialogoManager != null)
                dialogoManager.CumplirCondicion("interactuoF");
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
