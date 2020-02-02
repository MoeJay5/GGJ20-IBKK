using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tile : MonoBehaviour, ITurnListener
{
    private float rate = 5f;
    private float positionOffset = 0;
    private float scalingMagnitude = 0.9f;
    private Vector3 originalScale;
    private float speed = 0.5f;
    private float currentModifier = 0f;

    private TileStates currentState = TileStates.INACTIVE;

    public enum TileStates
    {
        INACTIVE,
        ACTIVE
    }

    void Start()
    {
        originalScale = transform.localScale;
        positionOffset = (transform.position.x * transform.position.x + transform.position.z + transform.position.z) /
                         16 * Mathf.PI;
        
        InitiativeSystem.registerListener(this);
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 7,
            1 << GameMaster.Layer_GridNode))
            hit.collider.gameObject.GetComponent<Node>().tile = this;

        //this.gameObject.SetActive(false);
    }

    public void setState(TileStates state)
    {
        currentState = state;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentState == TileStates.ACTIVE)
        {
            float modifier = Mathf.Cos(Time.time * rate + positionOffset) * 0.5f + 0.5f;
            modifier = (1.0f - scalingMagnitude) * modifier + scalingMagnitude;

            currentModifier = Mathf.MoveTowards(currentModifier, modifier, rate * Time.deltaTime);
            
            transform.localScale = originalScale * currentModifier;
        }
        else if(currentState == TileStates.INACTIVE)
        {
            currentModifier = Mathf.MoveTowards(currentModifier, 0, rate * Time.deltaTime);
            
            transform.localScale = originalScale * currentModifier;
        }
    }

    public void NextTurn(Unit unit)
    {
        // Reset State
    }
}
