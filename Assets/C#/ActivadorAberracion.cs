using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ActivarAberracion : MonoBehaviour
{
    public Volume globalVolume; // Arrastras el Global Volume aquí

    void OnEnable()
    {
        SetAberracion(1f);
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
