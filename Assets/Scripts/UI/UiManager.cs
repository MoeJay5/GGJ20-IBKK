using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Singleton
    public static UiManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Self References")]
    [SerializeField] private Image newCardShowoffPanel = null;
    [SerializeField] private Image newCardImage = null;
    [SerializeField] private Image noRoomForNewCardImage = null;
    [SerializeField] private Animator gameOverAnimator = null;
    [SerializeField] private GameObject levelVictoryScreen = null;
    [SerializeField] private GameObject gameVictoryScreen = null;

    // States
    private bool showingOffNewCard = false;
    private bool showingNoRoomForNewCard = false;

    /* Main Functionality */

    public void ShowOffNewCard(Sprite cardSprite, bool noRoomForNewCard)
    {
        showingOffNewCard = true;
        newCardShowoffPanel.gameObject.SetActive(true);
        newCardImage.sprite = cardSprite;

        if (noRoomForNewCard == false)
        {
            newCardImage.gameObject.SetActive(true);
        }
        else
        {
            showingNoRoomForNewCard = true;
            noRoomForNewCardImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (InputListener.Instance.PressedDown_Mouse_LeftClick)
        {
            if (showingNoRoomForNewCard)
                AcknowledgedFullHand();
            else if (showingOffNewCard)
                FinishShowingOffNewCard();
        }
    }

    private void AcknowledgedFullHand()
    {
        showingNoRoomForNewCard = false;
        noRoomForNewCardImage.gameObject.SetActive(false);
        newCardImage.gameObject.SetActive(true);
    }

    private void FinishShowingOffNewCard()
    {
        showingOffNewCard = false;
        newCardImage.gameObject.SetActive(false);
        newCardShowoffPanel.gameObject.SetActive(false);
    }

    //ToDo: Call when player hp hits 0
    public void GameOver()
    {
        gameOverAnimator.gameObject.SetActive(true);
        gameOverAnimator.SetTrigger("GameOver");
    }

    public void BackToTitleScreen()
    {
        LevelLoader.LoadSceneByName("TitleScene");
    }

    public void TryAgain()
    {
        LevelLoader.LoadSceneByName("Level 1");
    }

    public void LevelCompleted()
    {
        levelVictoryScreen.SetActive(true);
    }

    public void ToNextLevel(string nameOfLevelToLoad)
    {
        LevelLoader.LoadSceneByName(nameOfLevelToLoad);
    }

    public void GameCompleted()
    {
        gameVictoryScreen.SetActive(true);
    }
}
