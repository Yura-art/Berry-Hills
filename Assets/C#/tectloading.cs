using UnityEngine;
using TMPro;

public class LoadingBlink : MonoBehaviour
{
    public TMP_Text loadingText;
    public float blinkSpeed = 0.5f; // Velocidad de parpadeo (segundos)

    private void Start()
    {
        if (loadingText == null)
            loadingText = GetComponent<TMP_Text>();

        StartCoroutine(BlinkText());
    }

    private System.Collections.IEnumerator BlinkText()
    {
        while (true)
        {
            loadingText.enabled = !loadingText.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}

