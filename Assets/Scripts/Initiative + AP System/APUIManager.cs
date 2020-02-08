using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APUIManager : MonoBehaviour
{
    [SerializeField] private Image APMeter;

    [SerializeField] private List<Sprite> APMeterImages;

    private int currentAPValue;

    private int targetAPValue;

    private Unit currentTargetPlayer;

    [SerializeField] private float delayChange = 0.5f;
    private float lastChange = 0;
    
}
