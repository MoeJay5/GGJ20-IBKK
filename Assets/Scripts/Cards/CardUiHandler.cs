using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUiHandler : Card_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /* Variables */

    [Header("Dependencies")]
    [SerializeField] private Animator cardAnimator = null;
    [SerializeField] private Transform cardUsagePreviewDestination = null;

    // States
    private Vector3 positionInHand = Vector3.zero;
    private bool cardIsPreviewingUse = false;

    /* Main Functionality */

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardIsPreviewingUse)
            return;

        cardAnimator.SetBool("MousedOver", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardIsPreviewingUse)
            return;

        cardAnimator.SetBool("MousedOver", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(cardIsPreviewingUse == false)
            Animate_PreviewCardUsage(true);
    }

    /* Animation Helper Functions */

    private void Animate_PreviewCardUsage(bool moveToPreviewSpot)
    {
        cardIsPreviewingUse = true;
        cardAnimator.SetBool("CardUsagePreviewing", true);

        if (moveToPreviewSpot)
        {
            positionInHand = this.GetComponent<RectTransform>().position;
            StartCoroutine(LerpAnimation(this.GetComponent<RectTransform>(), positionInHand, cardUsagePreviewDestination.position));
        }
        else
            StartCoroutine(LerpAnimation(this.GetComponent<RectTransform>(), cardUsagePreviewDestination.position.normalized, positionInHand));
    }

    private IEnumerator LerpAnimation(RectTransform transform, Vector3 from, Vector3 to)
    {
        float lerpDuration = 1;
        float updateTickTime = 0.01f;
        float t = 0;

        while (t < lerpDuration)
        {
            transform.position = Vector3.Lerp(from, to, t);

            t += updateTickTime;
            yield return new WaitForSeconds(updateTickTime);
        }
    }
}
