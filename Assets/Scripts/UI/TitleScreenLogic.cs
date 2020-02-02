using UnityEngine;

public class TitleScreenLogic : MonoBehaviour
{
    /* Variables */

    [Header("References")]
    [SerializeField] private GameObject titlePage = null;
    [SerializeField] private GameObject creditsPage = null;

    // States
    private bool creditsPageShown = false;

    /* Initialization */

    private void Start()
    {
        EnableCreditsPage(false);
    }

    /* Main Functionality */

    public void EnableCreditsPage(bool creditsOn)
    {
        titlePage.SetActive(!creditsOn);
        creditsPage.SetActive(creditsOn);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
