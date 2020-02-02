using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //InitiativeSystem.registerManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator transitionTurns()
    {
        yield return new WaitForSeconds(1f);
        
        InitiativeSystem.finishNextTurn();
    }

    public void TriggerInitiativeChange()
    {
        StartCoroutine(transitionTurns());
    }
}
