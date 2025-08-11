using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;             // Velocidad normal de caminar
    public float velocidadCorrer = 8f;       // Velocidad al correr (Shift)
    private Rigidbody rb;                    // Referencia al Rigidbody para movimiento físico

    private Animator animator;               // Referencia al componente Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();      // Obtiene el Rigidbody del jugador
        animator = GetComponent<Animator>(); // Obtiene el Animator del jugador
    }

    void Update()
    {
        Mover();         // Movimiento horizontal y animaciones de caminar/correr
        RotarSegunMovimiento(); // Rota hacia la dirección del movimiento si se mueve
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Teclas A/D o flechas
        float vertical = Input.GetAxis("Vertical");     // Teclas W/S o flechas

        // Dirección basada en el input (en plano XZ), invertida para moverse al contrario
        Vector3 direccion = new Vector3(-horizontal, 0f, -vertical);

        if (direccion.magnitude < 0.1f)
        {
            animator.SetFloat("Velocidad", 0f);
            animator.SetBool("Corriendo", false);
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        float velocidadFinal = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidad;

        Vector3 movimiento = direccion.normalized * velocidadFinal;

        rb.velocity = new Vector3(movimiento.x, rb.velocity.y, movimiento.z);

        animator.SetFloat("Velocidad", movimiento.magnitude);
        animator.SetBool("Corriendo", Input.GetKey(KeyCode.LeftShift));
    }


    void RotarSegunMovimiento()
    {
        Vector3 velocidadHorizontal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (velocidadHorizontal.sqrMagnitude > 0.01f)
        {
            // Apunta hacia la dirección del movimiento
            Quaternion rotacionObjetivo = Quaternion.LookRotation(velocidadHorizontal);
            // Rotación suave
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }
    }
}
