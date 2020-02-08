using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!InitiativeSystem.currentUnit().IsPlayable) return;
        if (!InitiativeSystem.currentUnit().IsCurrentTurn) return;
        InitiativeSystem.nextTurn();
    }
}
