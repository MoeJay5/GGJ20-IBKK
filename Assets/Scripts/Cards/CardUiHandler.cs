using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class CardUiHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /* Variables */

    [Header("Dependencies")]
    [SerializeField] private Animator cardAnimator = null;
    public Card_ScriptableObject cardRef;
    [SerializeField] private static GraphicRaycaster UIRaycaster;
    
    // States
    private Vector3 positionInHand = Vector3.zero;
    public bool cardIsPreviewingUse = false;
    private bool cardIsMoving = false;
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

        if (cardIsPreviewingUse == false)
            Animate_PreviewCardUsage(true);
    }

    public void Update()
    {
        if (cardIsPreviewingUse && !cardIsMoving &&
            (InputListener.Instance.PressedDown_Escape || (InputListener.Instance.PressedDown_Mouse_LeftClick && MousedOverCardOrGrid() == false)))
            Animate_PreviewCardUsage(false);
        else if (InputListener.Instance.PressedUp_Mouse_LeftClick)
            cardRef.UseCard(InitiativeSystem.currentUnit(), InitiativeSystem.currentUnit().GetMyGridNode());
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
        cardIsMoving = true;
        float lerpDuration = 1;
        float updateTickTime = 0.01f;
        float t = 0;

        while (t < lerpDuration)
        {
            transform.position = Vector3.Lerp(from, to, t);

            t += updateTickTime;
            yield return new WaitForSeconds(updateTickTime);
        }
        cardIsMoving = false;
    }

    //ToDo
    private bool MousedOverCardOrGrid()
    {
        if (UIRaycaster == null)
            UIRaycaster = FindObjectOfType<GraphicRaycaster>();
        //card logic
        
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        UIRaycaster.Raycast(data, results);
        foreach (var hit in results)
        {
            if (hit.gameObject.GetComponent<Card_Base>())
                {
                return true;
            }
        }


        //grid Logic
        if (NodeMousedOver() != null)
            return true;
        
        return false;
    }

    /* Animation Events */
    public static Node NodeMousedOver()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(LevelStateManager.Instance.gameCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 50, 1 << GameMaster.Layer_GridNode))
        {
            return rayHit.collider.gameObject.GetComponent<Node>();
            
        }
        return null;
    }
    public void PlaySFX_OnHover()
    {
        AudioManager.Instance.PlayAudio(AudioManager.CardSfx.OnHover);
    }
}
