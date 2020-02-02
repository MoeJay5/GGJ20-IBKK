using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUiHandler : Card_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /* Variables */

    [Header("Dependencies")]
    [SerializeField] private Animator cardAnimator = null;

    // States
    private Vector3 positionInHand = Vector3.zero;
    private bool cardIsPreviewingUse = false;

    // Private vars
    private Coroutine cardPreviewUsageCoroutine = null;

    /* Main Functionality */

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardIsPreviewingUse)
            return;

        //AudioManager.Instance.PlayAudio(AudioManager.CardSfx.OnHover); - CALLED IN ANIMATION KEY
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
        if (HandCardManager.Instance.CurrentlySelectedCard != null)
            return;

        if(cardIsPreviewingUse == false)
            Animate_PreviewCardUsage(true);
    }

    public void Update()
    {
        if (cardIsPreviewingUse && InputListener.Instance.PressedDown_Escape)
            Animate_PreviewCardUsage(false);
    }

    /* Animation Helper Functions */

    private void Animate_PreviewCardUsage(bool moveToPreviewSpot)
    {
        HandCardManager.Instance.SetCurrentlySelectedCard(moveToPreviewSpot ? this : null);
        Transform cardUsagePreviewDestination = CardUsagePreviewPosition.Instance.transform;

        cardIsPreviewingUse = moveToPreviewSpot;
        cardAnimator.SetBool("CardUsagePreviewing", moveToPreviewSpot);

        if (cardPreviewUsageCoroutine != null)
        {
            StopCoroutine(cardPreviewUsageCoroutine);
            cardPreviewUsageCoroutine = null;
        }

        if (moveToPreviewSpot)
        {
            AudioManager.Instance.PlayAudio(AudioManager.CardSfx.OnPreviewUsage);

            positionInHand = this.GetComponent<RectTransform>().position;
            cardPreviewUsageCoroutine = StartCoroutine(LerpAnimation(this.GetComponent<RectTransform>(), positionInHand, cardUsagePreviewDestination.position));
        }
        else
        {
            cardAnimator.SetBool("MousedOver", false);
            StartCoroutine(LerpAnimation(this.GetComponent<RectTransform>(), cardUsagePreviewDestination.position, positionInHand));
        }
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

    /* Animation Events */

    public void PlaySFX_OnHover()
    {
        AudioManager.Instance.PlayAudio(AudioManager.CardSfx.OnHover);
    }
}
