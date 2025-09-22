using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicoSonido : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.instance != null && AudioManager.instance.peridico != null)
        {
            AudioManager.instance.ReproducirSonido(AudioManager.instance.peridico);
        }
    }
}
