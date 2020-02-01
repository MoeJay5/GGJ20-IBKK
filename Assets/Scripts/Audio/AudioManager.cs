using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Variables */

    [Header("Dependencies")]
    [SerializeField] private AudioSource audioPlayer = null;

    /* Sounds */

    public enum CardSfx { OnHover = 0, OnPreviewUsage = 1, OnUse = 2 }

    [Header("Card SFX")]
    [SerializeField] private AudioClip[] cardSfx = new AudioClip[0];

    /* Helper Functionality */

    public void PlayAudio(CardSfx cardAudio)
    {
        audioPlayer.clip = cardSfx[(int)cardAudio];
        audioPlayer.Play();
    }
}
