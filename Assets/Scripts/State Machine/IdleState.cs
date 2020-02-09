using System.Collections.Generic;
using UnityEngine;

class IdleState : State
{
    private Unit myUnit;

    public override void EnterState (StateMachine parent)
    {
        myUnit = this.GetComponent<Unit> ();
        if (myUnit == null)
        {
            Debug.LogWarning ("State assigned to state machine without unit.");
        }
    }

    public override void UpdateState (StateMachine parent)
    {
        if (!myUnit.IsCurrentTurn) return;
        InitiativeSystem.nextTurn();
    }
}