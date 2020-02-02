using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Singleton & DontDestroyOnLoad
    public static GameStateManager Instance;
    private void OnEnable()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion
}
