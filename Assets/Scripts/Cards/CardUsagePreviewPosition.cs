using UnityEngine;

public class CardUsagePreviewPosition : MonoBehaviour
{
    #region Singleton
    public static CardUsagePreviewPosition Instance;
    private void OnEnable() => Instance = this;
    #endregion
}
