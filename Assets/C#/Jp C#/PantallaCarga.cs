using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaCarga : MonoBehaviour
{
    public Image loadingBar;

    void Start()
    {
        StartCoroutine(LoadAsync(GameManager.sceneToLoad));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            if (loadingBar != null)
                loadingBar.fillAmount = operation.progress;
            yield return null;
        }

        yield return new WaitForSeconds(3); // Espera para transición

        operation.allowSceneActivation = true;
    }   
}
