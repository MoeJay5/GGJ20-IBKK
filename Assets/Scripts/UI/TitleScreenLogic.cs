using UnityEngine;

public class TitleScreenLogic : MonoBehaviour
{
    /* Variables */

    [Header("Title \"Pages\"")]
    [SerializeField] private GameObject titlePage = null;
    [SerializeField] private GameObject creditsPage = null;

    /* Initialization */

    private void Start()
    {
        EnableCreditsPage(false);
    }

    /* Main Functionality */

    public void PlayGame()
    {
        LevelLoader.LoadSceneByName("Level1");
    }

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
