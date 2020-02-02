using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    /* Variables */

    public bool completed = false;
    public bool Completed { get => completed; }

    /* Main Functionality */

    public void ObjectiveCompleted()
    {
        completed = true;
        LevelObjectiveSystem.Instance.ObjectiveCompleted();
    }
}
