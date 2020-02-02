using System.Collections.Generic;
using UnityEngine;

class IdleState : State
{
    private Unit myUnit;

    public override void EnterState (StateMachine parent)
    {
        myUnit = parent.GetComponent<Unit> ();
    }

    public override void UpdateState (StateMachine parent)
    {

    }
}