using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APUIManager : MonoBehaviour, ITurnListener
{
    [SerializeField] private Image APMeter;

    [SerializeField] private List<Sprite> APMeterImages;

    private int currentAPValue;

    private int targetAPValue;

    [SerializeField] private float delayChange = 0.5f;
    private float lastChange = 0;

    private void Start()
    {
        InitiativeSystem.registerListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        Unit u = InitiativeSystem.currentUnit();
        
        if (u == null) return;
        
        if (!u.CurrentTurn)
        {
            APMeter.sprite = APMeterImages[u.AP];
        };
        
        if (u.IsEnemy) return;

        targetAPValue = u.AP;

        if (currentAPValue != targetAPValue && (Time.time - lastChange) > delayChange)
        {
            lastChange = Time.time;
            currentAPValue += Math.Sign(targetAPValue - currentAPValue);
            APMeter.sprite = APMeterImages[currentAPValue];
        }
    }

    public void NextTurn(Unit unit)
    {
        currentAPValue = InitiativeSystem.currentUnit().previousAP;
        targetAPValue = InitiativeSystem.currentUnit().AP;
        APMeter.sprite = APMeterImages[currentAPValue];
    }
}
