using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Singleton
    public static GameStateManager Instance;
    private void OnEnable() => Instance = this;
    #endregion
}
