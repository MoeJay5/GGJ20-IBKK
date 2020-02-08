using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (InitiativeSystem.currentUnit().IsEnemy) return;
        if (!InitiativeSystem.currentUnit().CurrentTurn) return;
        InitiativeSystem.nextTurn();
    }
}
