using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;

    private Rigidbody rb;
    private Animator animator;

    [Header("Estado")]
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
    }

    void Mover()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(-horizontal, 0f, -vertical);

        if (direccion.sqrMagnitude < 0.01f)
        {
            DetenerProcesosMovimiento();
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
            if (!corrio) corrio = true;
            ReproducirSonido(AudioManager.instance?.correr, AudioManager.instance?.caminar);
        }
        else
        {
            if (!camino) camino = true;
            ReproducirSonido(AudioManager.instance?.caminar, AudioManager.instance?.correr);
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

    // --- MÉTODOS PARA UNITY EVENT ---

    public void BloquearMovimiento()
    {
        puedeMover = false;
        if (rb != null) rb.isKinematic = true; // Activa Kinematic si otro script o evento lo necesita
        DetenerProcesosMovimiento();
    }

    public void DesbloquearMovimiento()
    {
        puedeMover = true;
        if (rb != null) rb.isKinematic = false; // Desactiva Kinematic para devolver el control al Rigidbody
    }

    private void DetenerProcesosMovimiento()
    {
        if (animator != null)
        {
            animator.SetFloat("Velocidad", 0f);
            animator.SetBool("Corriendo", false);
        }

        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        DetenerSonidos();
    }

    void DetenerSonidos()
    {
        if (AudioManager.instance != null)
        {
            if (AudioManager.instance.caminar != null) AudioManager.instance.caminar.Stop();
            if (AudioManager.instance.correr != null) AudioManager.instance.correr.Stop();
        }
    }

    void ReproducirSonido(AudioSource aReproducir, AudioSource aDetener)
    {
        if (aReproducir == null || aDetener == null) return;

        if (!aReproducir.isPlaying)
        {
            aDetener.Stop();
            aReproducir.Play();
        }
    }
}