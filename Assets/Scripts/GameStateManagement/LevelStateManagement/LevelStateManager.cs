using System.Collections;
using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    #region Singleton
    public static LevelStateManager Instance;
    private void OnEnable () => Instance = this;
    #endregion


    [Header ("Dependencies")]
    public Camera gameCamera;

    private GridSystem _gridSystem;
    
    public GridSystem gridSystem
    {
        get
        {
            if (_gridSystem == null)
            {
                Debug.LogWarning("Level State Manager did not have a grid system reference defined. Attempting to find one.");
                _gridSystem = FindObjectOfType<GridSystem>();
            }


            if (_gridSystem == null)
            {
                Debug.LogError("Level is missing a grid system.");
            }

            return _gridSystem;
        }

        set { _gridSystem = value; }
    }

    /* Level Initialization */

    private void Start()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
    }
}