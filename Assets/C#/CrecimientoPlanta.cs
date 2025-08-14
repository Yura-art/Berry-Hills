using System;
using UnityEngine;

public class CrecimientoPlanta : ObjetoLlevable
{
    private Regadera regaderaEnZona;      // Referencia a la regadera si está en zona de la planta
    private float agua = 0f;              // Cantidad de agua acumulada para el crecimiento

    public float aguaPorEtapa = 5f;      // Agua necesaria para avanzar una etapa de crecimiento

    public int etapaActual = 0;           // Etapa actual de crecimiento de la planta
    public int etapaMaxima = 3;           // Etapa máxima que puede alcanzar
    public Animator animator;             // Animator para controlar animaciones de crecimiento

    private void Start()
    {
        puedeCargar = false;
    }
    public override void xD()
    {
        Debug.Log("Crecimiento planta");
    }

    public override void InteractuarClick(GameObject interactor)
    {
        // Cuando se hace click para interactuar, se intenta regar si hay regadera en zona
        if (regaderaEnZona != null)
        {
            regaderaEnZona.Regar(this);   // Llama al método Regar de la regadera, pasándole esta planta
        }
    }

    public void RecibirAgua(float cantidad)
    {
        // Si la planta ya está en etapa máxima, no hace nada
        if (etapaActual >= etapaMaxima) return;

        agua += cantidad;  // Acumula la cantidad de agua recibida

        // Mientras haya suficiente agua para avanzar etapas, crece
        if (agua >= aguaPorEtapa && etapaActual < etapaMaxima)
        {
            agua = 0;  // Resta el agua usada para crecer
            Crecer();              // Llama al método para avanzar etapa
        }
    }

    private void Crecer()
    {
        // Incrementa la etapa actual sin pasarse del máximo permitido
        etapaActual = Mathf.Min(etapaActual + 1, etapaMaxima);

        // Actualiza parámetro en el Animator para cambiar animación
        animator.SetInteger("Etapa", etapaActual);

        if (AudioManager.instance != null && AudioManager.instance.cosechar != null)
        {
            AudioManager.instance.ReproducirSonido(AudioManager.instance.cosechar);
        }

        // Si alcanzó la etapa máxima, se convierte en objeto llevable
        if (etapaActual == etapaMaxima)
        {
            // Solo agrega componentes si no los tiene ya
            if (!puedeCargar)
            {
                puedeCargar = true;

                Rigidbody rb = gameObject.AddComponent<Rigidbody>(); // Añade Rigidbody para físicas
                rb.mass = 1f;               // Masa del objeto
                rb.useGravity = true;       // Usa gravedad
                rb.isKinematic = false;     // Física activa

                // Busca un BoxCollider existente para eliminarlo y evitar duplicados o problemas
                BoxCollider boxColliderExistente = GetComponent<BoxCollider>();

                if (boxColliderExistente != null)
                {
                    Destroy(boxColliderExistente);
                }

                // Añade un nuevo BoxCollider configurado
                BoxCollider nuevoBoxCollider = gameObject.AddComponent<BoxCollider>();
                nuevoBoxCollider.size = new Vector3(3f, 3f, 3f);
                nuevoBoxCollider.center = Vector3.zero;
            }

            //// Programa la autodestrucción del script Planta después de 1 segundo
            //Invoke("Autodestruir", 1f);
        }
    }

    void Autodestruir()
    {
        // Destruye este componente Planta para evitar que siga funcionando
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando un collider entra en la zona de la planta, verifica si es una regadera
        var regadera = other.GetComponent<Regadera>();
        if (regadera != null)
        {
            regaderaEnZona = regadera;  // Guarda referencia a la regadera en zona
            Debug.Log("Regadera detectada en la planta.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando un collider sale de la zona, verifica si es la regadera que tenía registrada
        var regadera = other.GetComponent<Regadera>();
        if (regadera != null && regadera == regaderaEnZona)
        {
            regaderaEnZona = null;  // Elimina referencia porque la regadera ya no está
            Debug.Log("Regadera salió de la planta.");
        }
    }
}
