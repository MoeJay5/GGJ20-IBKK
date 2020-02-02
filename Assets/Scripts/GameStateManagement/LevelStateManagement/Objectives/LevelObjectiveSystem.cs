using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class LevelObjectiveSystem : MonoBehaviour
{
    #region Singleton
    public static LevelObjectiveSystem Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Properties")]
    [SerializeField] private List<LevelObjective> mandatoryObjectives = new List<LevelObjective>();
    [SerializeField] private string levelToLoadAfterThisOne = "Level 2";
    [SerializeField] private bool isLastLevel = false;

    // States
    private bool highlightedExit = false;

    /* Main Functionality */

    //Called by an objective
    public void ObjectiveCompleted()
    {
        foreach(var ob in mandatoryObjectives)
        {
            if(!ob.Completed)
            {
                ob.completed = true;
                break;
            }
        }
        if (highlightedExit == false && AllMandatoryObjectivesCompleted())
            CompletedLastMandatoryObjective();
    }

    /* Helper Functions */

    private void CompletedLastMandatoryObjective()
    {
        highlightedExit = true;
        StartCoroutine(WaitASec());
    }
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(2);
        UiManager.Instance.LevelCompleted(isLastLevel);

    }
    private bool AllMandatoryObjectivesCompleted()
    {
        foreach (LevelObjective mandatoryObjective in mandatoryObjectives)
        {
            if (mandatoryObjective.Completed == false)
                return false;
        }

        return true;
    }
}
