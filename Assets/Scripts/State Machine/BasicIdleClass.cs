using UnityEngine;

class BasicIdleClass : State
{
    [SerializeField] private Animator animator;
    public override void UpdateState(StateMachine parent)
    {
        animator.SetBool("Idleing", true);
        InitiativeSystem.nextTurn();
    }
}