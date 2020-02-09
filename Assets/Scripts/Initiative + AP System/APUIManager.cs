using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class APUIManager : MonoBehaviour
{
    [SerializeField] private Image APMeter;
    [SerializeField] private List<Sprite> APMeterImages;
    [SerializeField] private float changeDelay = 0.1f;

    private int currentAPValue = 5;
    private float lastChange = 0;

    public void NextTurn(Unit unit)
    {
        APMeter.color = unit.IsPlayable ? Color.white : Color.grey;
        lastChange = Time.time;
        currentAPValue = unit.currentAP;
    }

    void Update()
    {
        if (Time.time - lastChange > changeDelay)
        {
            int deltaValue = Mathf.Clamp( InitiativeSystem.currentUnit().currentAP - currentAPValue, -1, 1);
            currentAPValue = Mathf.Clamp(currentAPValue + deltaValue, 0, 5);
            APMeter.sprite = APMeterImages[currentAPValue];
            lastChange = Time.time;
        }
    }
}
