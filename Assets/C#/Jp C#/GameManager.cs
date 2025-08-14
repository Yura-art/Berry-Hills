using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string sceneToLoad;

    [Header("UI")]
    public GameObject panelPausa;
    public GameObject panelPerdio;

    private bool juegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                ReanudarJuego();
            else
                PausarJuego();
        }
    }


    public static void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        sceneToLoad = sceneName;
        SceneManager.LoadScene("Carga");
    }

    // --- PAUSA ---
    public void PausarJuego()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void ReanudarJuego()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    // --- PERDER ---
    public void MostrarPantallaPerdio()
    {
        panelPerdio.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- BOTONES ---
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        LoadScene("Inicio"); 
    }

    public void CerrarJuego()
    {
        Debug.Log("Se cerro");
        Application.Quit();
    }
}
