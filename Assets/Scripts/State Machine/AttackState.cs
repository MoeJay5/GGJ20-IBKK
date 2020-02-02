using System.Collections.Generic;
using UnityEngine;

class AttackState : State
{
    private Unit myUnit;

    public override void EnterState (StateMachine parent)
    {
        myUnit = parent.GetComponent<Unit> ();
    }

    public override void UpdateState (StateMachine parent)
    {
        
    }

    public override void ExitState (StateMachine parent)
    {

    }
}