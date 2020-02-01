using System.Collections;
using UnityEngine;

public abstract class Card_Base : MonoBehaviour
{
    protected virtual void ActivateCard()
    {
        //ToDo: Card Effect
    }

    private IEnumerator PlayCardVFX()
    {
        //ToDo VFX

        bool doneWithVFX = true;

        yield return new WaitUntil(() => doneWithVFX == true);

        DeleteCard();
    }

    private void DeleteCard()
    {
        GameObject.Destroy(this);
    }
}
