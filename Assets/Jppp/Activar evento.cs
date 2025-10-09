using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activarevento : MonoBehaviour
{
    [SerializeField] List<string> tagPermitido;
    [SerializeField] UnityEvent eventosAlEntrar;
    [SerializeField] UnityEvent eventosAlSalir;

    private void OnTriggerEnter(Collider other)
    {
        if (tagPermitido.Contains(other.tag))
        {
            eventosAlEntrar?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tagPermitido.Contains(other.tag))
        {
            eventosAlSalir?.Invoke();
        }
    }
}
