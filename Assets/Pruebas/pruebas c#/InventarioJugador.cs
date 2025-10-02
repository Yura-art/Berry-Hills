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

    // Propiedades públicas de solo lectura
    public ObjetoInteractuable ObjetoEnMano => objetoEnMano;
    public int SlotActivo => slotActivo;

    private void Update()
    {
        // Cambiar de slot con teclas 1, 2, 3
        if (Input.GetKeyDown(KeyCode.Alpha1)) CambiarSlot(0);

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

        // Mensajes de UI
        if (objetoEnMano != null)
        {
            //UIInventario.Instance.MostrarMensaje("Presiona E para interactuar con " + objetoEnMano.nombre);
        }
        else if (objetoCerca == null)
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
        }

    }

    // Suelta un objeto del inventario
    public void SoltarObjeto(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        if (slots[slotIndex] == null) return;

        ObjetoInteractuable objeto = slots[slotIndex];
        objeto.transform.SetParent(null);

        Vector3 posicionSoltar = transform.position + transform.forward * 5f;
        objeto.Soltar(posicionSoltar);
        objeto.gameObject.SetActive(true);

        // Ajuste para objetos con Rigidbody
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        UIInventario.Instance.MostrarMensaje("Soltaste " + objeto.nombre);

        slots[slotIndex] = null;
        if (slotActivo == slotIndex) objetoEnMano = null;
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

    }

    // Define el objeto cercano
    public void SetObjetoCerca(ObjetoInteractuable obj)
    {
        objetoCerca = obj;
    }
}
