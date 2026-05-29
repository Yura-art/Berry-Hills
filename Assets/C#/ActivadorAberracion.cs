using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ActivarAberracion : MonoBehaviour
{
    public Volume globalVolume; // Arrastras el Global Volume aquí
    public float aberracion;
    void OnEnable()
    {
        SetAberracion(aberracion);
    }

    void OnDisable()
    {
        SetAberracion(0f);
    }

    void SetAberracion(float valor)
    {
        ChromaticAberration aberracion;
        globalVolume.profile.TryGet(out aberracion);
        aberracion.intensity.value = valor;
    }
}
