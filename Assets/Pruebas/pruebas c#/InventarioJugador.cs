using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    [Header("Slots del inventario")]
    public ObjetoInteractuable[] slots = new ObjetoInteractuable[3]; // Máximo 3 espacios

    [Header("Referencia donde se equipa el objeto en la mano")]
    public Transform puntoMano;

    private ObjetoInteractuable objetoCerca;   // Objeto cercano que se puede recoger
    private ObjetoInteractuable objetoEnMano;  // Objeto actualmente en la mano
    private int slotActivo = -1;               // Índice del slot activo
    private Animator animator;                 // Animator del jugador

    // Propiedades públicas de solo lectura
    public ObjetoInteractuable ObjetoEnMano => objetoEnMano;
    public int SlotActivo => slotActivo;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Cambiar de slot con teclas 1, 2, 3
        if (Input.GetKeyDown(KeyCode.Alpha1)) CambiarSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CambiarSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CambiarSlot(2);

        // Recoger o soltar objetos con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (objetoCerca != null) // Recoger
            {
                AgregarObjeto(objetoCerca);
                UIInventario.Instance.MostrarMensaje("");
                objetoCerca = null;
            }
            else if (objetoEnMano != null) // Soltar
            {
                SoltarObjeto(slotActivo);
            }
        }

        // Usar el objeto en mano con E
        if (Input.GetKeyDown(KeyCode.E) && objetoEnMano != null)
        {
            objetoEnMano.Usar();
        }

        // Limpiar mensajes si no hay nada
        if (objetoEnMano == null && objetoCerca == null)
        {
            UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Agrega un objeto al inventario
    public void AgregarObjeto(ObjetoInteractuable obj)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // Encuentra un slot vacío
            {
                slots[i] = obj;
                obj.Recoger();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(false);

                if (slotActivo == -1 || slotActivo == i)
                {
                    CambiarSlot(i);
                }

                UIInventario.Instance.MostrarMensaje(obj.nombre + " agregado al inventario");
                AudioManager.instance.ReproducirSonido(AudioManager.instance.tomarObjeto);

                return;
            }
        }

        // Si no hay espacio
        UIInventario.Instance.MostrarMensaje("Inventario lleno!");
    }

    // Cambia de slot activo
    public void CambiarSlot(int nuevoSlot)
    {
        if (nuevoSlot < 0 || nuevoSlot >= slots.Length) return;

        // Quitar objeto en mano si hay
        if (objetoEnMano != null)
        {
            objetoEnMano.gameObject.SetActive(false);
            objetoEnMano.transform.SetParent(null);
            objetoEnMano = null;
        }

        slotActivo = nuevoSlot;

        if (slots[slotActivo] != null)
        {
            objetoEnMano = slots[slotActivo];
            objetoEnMano.gameObject.SetActive(true);
            objetoEnMano.transform.SetParent(puntoMano);
            objetoEnMano.transform.localPosition = Vector3.zero;
            objetoEnMano.transform.localRotation = Quaternion.identity;

            animator.SetBool("Interactuando", true); // ✅ activar animación
        }
        else
        {
            animator.SetBool("Interactuando", false); // ✅ desactivar si no hay objeto
        }
    }

    // Suelta un objeto del inventario
    public void SoltarObjeto(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        if (slots[slotIndex] == null) return;

        ObjetoInteractuable objeto = slots[slotIndex];

        // ✅ Volver a posición y rotación original
        objeto.Soltar();

        UIInventario.Instance.MostrarMensaje("Soltaste " + objeto.nombre);
        AudioManager.instance.ReproducirSonido(AudioManager.instance.tomarObjeto);

        // ✅ Quitar del inventario
        slots[slotIndex] = null;

        // ✅ Quitar de la mano si era el activo
        if (slotActivo == slotIndex)
        {
            objetoEnMano = null;
            animator.SetBool("Interactuando", false);
        }
    }



    // Usa y elimina el objeto activo del inventario
    public void UsarObjetoActivo()
    {
        if (slotActivo < 0 || slotActivo >= slots.Length) return;
        if (slots[slotActivo] == null) return;

        ObjetoInteractuable usado = slots[slotActivo];
        slots[slotActivo] = null;
        objetoEnMano = null;

        if (usado != null)
        {
            usado.transform.SetParent(null);
            Destroy(usado.gameObject);
        }

        animator.SetBool("Interactuando", false); // ✅ desactivar animación
    }

    // Define el objeto cercano
    public void SetObjetoCerca(ObjetoInteractuable obj)
    {
        objetoCerca = obj;
        // 🔊 Si quieres sonido aquí, lo dejamos
        // AudioManager.instance.ReproducirSonido(AudioManager.instance.tomarObjeto);
    }
}
