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

    void Start()
    {
        originalScale = transform.localScale;
        positionOffset = (transform.position.x * transform.position.x + transform.position.z + transform.position.z) /
                         16 * Mathf.PI;
        
        InitiativeSystem.registerListener(this);
    }


    // Update is called once per frame
    void Update()
    {
        float modifier = Mathf.Cos(Time.time * rate + positionOffset) * 0.5f + 0.5f;
        modifier = (1.0f - scalingMagnitude) * modifier + scalingMagnitude;
        transform.localScale = originalScale * modifier;
    }

    public void NextTurn(Unit unit)
    {
        // Reset State
    }
}
