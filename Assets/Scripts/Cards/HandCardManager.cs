using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    #region Singleton
    public static HandCardManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    // Private Vars
    private CardUiHandler currentlySelectedCard = null;
    public CardUiHandler CurrentlySelectedCard { get => currentlySelectedCard; }

    public void SetCurrentlySelectedCard(CardUiHandler newlySelectedCard) => currentlySelectedCard = newlySelectedCard;
}
