using UnityEngine;
using UnityEngine.EventSystems;

public class BotonSonido : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReproducirBotonHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReproducirBotonClick();
        }
    }
}
