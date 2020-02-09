using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class State : MonoBehaviour {
    public virtual void EnterState(StateMachine parent)
    {
    }
    
    public virtual void ExitState(StateMachine parent)
    {
    }
    
    public virtual void UpdateState(StateMachine parent)
    {
    }

    public virtual void PauseState(StateMachine parent)
    {
        
    }

    public virtual void UnPauseState(StateMachine parent)
    {
        
    }
}