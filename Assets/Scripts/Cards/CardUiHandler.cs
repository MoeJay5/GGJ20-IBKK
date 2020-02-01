using UnityEngine;
using UnityEngine.EventSystems;

public class CardUiHandler : Card_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /* Variables */

    [Header("Dependencies")]
    [SerializeField] private Animator cardAimator = null;

    /* Main Functionality */

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardAimator.SetBool("MousedOver", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardAimator.SetBool("MousedOver", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
