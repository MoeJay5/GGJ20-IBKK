using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] Stack<State>  stateStack = new Stack<State>();

    [SerializeField] State defaultState;
    [SerializeField] State startState;

    [SerializeField] private Unit assignedUnit;

    // Start is called before the first frame update
    void Start()
    {
        if (startState != null)
        {
            PushState(startState);
        }
        else
        {
            PushState(defaultState);
        }

        if (assignedUnit == null)
        {
            assignedUnit = GetComponent<Unit>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (assignedUnit != null && !assignedUnit.IsCurrentTurn) return;
        stateStack.Peek().UpdateState(this);
    }

    public void PushState(State s)
    {
        if (s == null)
        {
            Debug.LogWarning("Tried to add null state.");
            return;
        }
        
        if (stateStack.Count > 0)
        {
            stateStack.Peek().PauseState(this);
        }

        stateStack.Push(s);
        stateStack.Peek().EnterState(this);
    }

    public void PopState()
    {
        if (stateStack.Count > 0)
        {
            stateStack.Peek().ExitState(this);
            stateStack.Pop();
        }

        if (stateStack.Count == 0)
        {
            PushState(defaultState);
        }
        else
        {
            stateStack.Peek().UnPauseState(this);
        }
    }

    public void ReplaceState(State s)
    {
        if (s == null)
        {
            Debug.LogWarning("Tried to add null state.");
            return;
        }

        if (stateStack.Count > 0)
        {
            stateStack.Peek().ExitState(this);
            stateStack.Pop();
        }
        
        stateStack.Push(s);
        stateStack.Peek().EnterState(this);
        
    }
}