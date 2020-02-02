using UnityEngine;

class BasicIdleClass : State
{
    public override void UpdateState(StateMachine parent)
    {
        InitiativeSystem.nextTurn();
    }
}