using UnityEngine;
using UnityEngine.UI;

public class HealthUiManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image hpBarImage = null;

    [Header("Art Assets")]
    [SerializeField] private Sprite[] healthBarSprites = null; //[0] = 0 health shown, [5] = 5 of 6 health shown

    public void UpdateHealthUi(int newCurrHealthValue)
    {
        if (newCurrHealthValue >= healthBarSprites.Length)
            newCurrHealthValue = healthBarSprites.Length - 1;

        hpBarImage.sprite = healthBarSprites[newCurrHealthValue];
    }
}
