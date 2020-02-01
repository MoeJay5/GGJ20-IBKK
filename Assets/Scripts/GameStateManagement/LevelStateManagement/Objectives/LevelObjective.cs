using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    /* Variables */

    private bool completed = false;
    public bool Completed { get => completed; }

    /* Main Functionality */

    protected virtual void ObjectiveCompleted()
    {
        completed = true;
        LevelObjectiveSystem.Instance.ObjectiveCompleted();
    }
}
