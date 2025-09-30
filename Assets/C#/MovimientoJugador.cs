using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    private Rigidbody rb;
    private Animator animator;
    public bool puedeMover = true;

    private bool camino = false;
    private bool corrio = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (puedeMover)
        {
            Mover();
            RotarSegunMovimiento();
        }
        else
        {
            // Detener al jugador si no puede moverse
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            animator.SetFloat("Velocidad", 0f);
            animator.SetBool("Corriendo", false);
            DetenerSonidos();
        }
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(-horizontal, 0f, -vertical);

        if (direccion.magnitude < 0.1f)
        {
            BloquearMovimiento();
            return;
        }

        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift);
        float velocidadFinal = estaCorriendo ? velocidadCorrer : velocidad;
        Vector3 movimiento = direccion.normalized * velocidadFinal;
        rb.velocity = new Vector3(movimiento.x, rb.velocity.y, movimiento.z);

        animator.SetFloat("Velocidad", movimiento.magnitude);
        animator.SetBool("Corriendo", estaCorriendo);

        if (estaCorriendo)
        {
            if (!corrio)
            {
                corrio = true;
                FindObjectOfType<DialogoManager>().CumplirCondicion("correrRealizado");
            }
            ReproducirSonido(AudioManager.instance.correr, AudioManager.instance.caminar);
        }
        else
        {
            if (!camino)
            {
                camino = true;
                FindObjectOfType<DialogoManager>().CumplirCondicion("movimientoRealizado");
            }
            ReproducirSonido(AudioManager.instance.caminar, AudioManager.instance.correr);
        }
    }


    void RotarSegunMovimiento()
    {
        Vector3 velocidadHorizontal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (velocidadHorizontal.sqrMagnitude > 0.01f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(velocidadHorizontal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
        }
    }

    void DetenerSonidos()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.caminar.Stop();
            AudioManager.instance.correr.Stop();
        }
    }

    void ReproducirSonido(AudioSource aReproducir, AudioSource aDetener)
    {
        if (!aReproducir.isPlaying)
        {
            aDetener.Stop();
            aReproducir.Play();
        }
    }

    public void BloquearMovimiento()
    {
        animator.SetFloat("Velocidad", 0f);
        animator.SetBool("Corriendo", false);
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

        DetenerSonidos();
        return;
    }
}
