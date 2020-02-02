using System.Collections.Generic;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    #region Singleton
    public static HandCardManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Cards")]
    [SerializeField] private List<CardUiHandler> myCards = new List<CardUiHandler>();

    [Header("Properties")]
    [SerializeField] private float cardSpacingMultiplier = 1.0f;
    [SerializeField, Range(3, 20)] private int maxNumberCards = 7;

    // Private Vars
    private CardUiHandler currentlySelectedCard = null;
    public CardUiHandler CurrentlySelectedCard { get => currentlySelectedCard; }

    public void SetCurrentlySelectedCard(CardUiHandler newlySelectedCard) => currentlySelectedCard = newlySelectedCard;

    /* Main Functionality */

    public void InitializeHandForLevel()
    {
        int i = 1;

        List<CardUiHandler> cards = myCards;
        while(cards.Count > maxNumberCards)
            cards.RemoveAt(cards.Count - 1);

        foreach (CardUiHandler card in cards)
        {
            RectTransform spawnedCard = GameObject.Instantiate(card.gameObject, HandCardsParentTransform.Instance.transform).GetComponent<RectTransform>();

            Vector3 localPos = spawnedCard.localPosition;
            localPos.x = GetCardSpawnPosition(Mathf.Clamp(myCards.Count, 0, maxNumberCards), i++);
            spawnedCard.localPosition = localPos * 100;
        }
    }

    //ToDo: Call this function when you USE a card
    public void RemoveCardFromHand(CardUiHandler cardToRemove)
    {
        myCards.Remove(cardToRemove);
    }

    //ToDo: Call this when getting a new card
    public void GainNewCard(CardUiHandler cardGained)
    {
        if (myCards.Count >= maxNumberCards)
            GainedNewCardWithNoRoomLeft();

        myCards.Add(cardGained);
    }
    private void GainedNewCardWithNoRoomLeft()
    {
        //ToDo
    }

    /* Helper Functions */

    private float GetCardSpawnPosition(int totalNumCards, int thisCardNum) //thisCardNum = [1, totalNumCards]
    {
        return ((2.0f / (totalNumCards + 1) * thisCardNum) - 1f) * cardSpacingMultiplier;
    }
}