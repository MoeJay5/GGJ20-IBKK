using UnityEngine;

class BasicWalkState : State
{
    [SerializeField] private float distance;
    [SerializeField] private float duration;

    private float startTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    
    public override void EnterState(StateMachine parent)
    {
        startTime = Time.fixedTime;
        startPosition = parent.transform.position;
        startPosition = parent.transform.position + Vector3.forward * distance;
    }
    
    public override void  UpdateState(StateMachine parent)
    {
        Debug.Log("Update!");
        
        if (Time.time > startTime + duration)
        {
            parent.gameObject.transform.position = endPosition;
            parent.PopState();
        }
        
        Debug.Log("Update!");

        parent.transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) / duration);
    }
}