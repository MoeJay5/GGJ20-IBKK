using UnityEngine;

public class HandCardsParentTransform : MonoBehaviour
{
    #region Singleton
    public static HandCardsParentTransform Instance;
    private void OnEnable() => Instance = this;
    #endregion
}
