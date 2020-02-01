using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Singleton
    public static GameStateManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Temp References")]
    [SerializeField] private Unit[] units = new Unit[0];

    /* Initialization */

    private void Start()
    {
        UpdateAllUnitInitiatives();
    }

    /* Helper Functions */

    private void UpdateAllUnitInitiatives()
    {
        foreach (Unit unit in units)
        {
            unit.SetRandomInitiative();
        }
    }
}
