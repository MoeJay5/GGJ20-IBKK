using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCardManager : MonoBehaviour
{
    #region Singleton
    public static HandCardManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Cards")]
    [SerializeField] private List<Card_ScriptableObject> myCards = new List<Card_ScriptableObject>();
    [SerializeField] private CardUiHandler cardUiPrefab = null;

    [Header("Properties")]
    [SerializeField] private float cardSpacingMultiplier = 1.0f;
    [SerializeField, Range(3, 20)] private int maxNumberCards = 7;


    // Private Vars
    private CardUiHandler currentlySelectedCard = null;
    public CardUiHandler CurrentlySelectedCard { get => currentlySelectedCard; }
    public static Texture BlueOutline => Instance.blueOutline;
    public static Texture RedOutline => Instance.redOutline;
    public static Texture WhiteOutline => Instance.whiteOutline;
    [SerializeField]private Texture redOutline;
    [SerializeField]private Texture blueOutline;
    [SerializeField] private Texture whiteOutline;
    public void SetCurrentlySelectedCard(CardUiHandler newlySelectedCard) => currentlySelectedCard = newlySelectedCard;

    /* Main Functionality */

    public void InitializeHandForLevel()
    {
        int i = 1;

        List<Card_ScriptableObject> cards = myCards;
        while(cards.Count > maxNumberCards)
            cards.RemoveAt(cards.Count - 1);

        foreach (Card_ScriptableObject card in cards)
        {
            RectTransform spawnedCard = GameObject.Instantiate(cardUiPrefab.gameObject, HandCardsParentTransform.Instance.transform).GetComponent<RectTransform>();
            spawnedCard.GetComponent<Image>().sprite = card.cardImage;

            Vector3 localPos = spawnedCard.localPosition;
            localPos.x = GetCardSpawnPosition(Mathf.Clamp(myCards.Count, 0, maxNumberCards), i++);
            spawnedCard.localPosition = localPos * 100;
            spawnedCard.GetComponent<CardUiHandler>().cardRef = card;
        }
    }

    //ToDo: Call this function when you USE a card
    public void RemoveCardFromHand(Card_ScriptableObject cardToRemove)
    {
        myCards.Remove(cardToRemove);
    }

    //ToDo: Call this when getting a new card
    public void GainNewCard(Card_ScriptableObject cardGained)
    {
        myCards.Add(cardGained);

        UiManager.Instance.ShowOffNewCard(cardGained.cardImage, myCards.Count > maxNumberCards);
    }

    /* Helper Functions */

    private float GetCardSpawnPosition(int totalNumCards, int thisCardNum) //thisCardNum = [1, totalNumCards]
    {
        return ((2.0f / (totalNumCards + 1) * thisCardNum) - 1f) * cardSpacingMultiplier;
    }
}