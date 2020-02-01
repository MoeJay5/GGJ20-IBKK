﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InitiativeSystem
{
    private static List<Unit> activeUnits = new List<Unit>();
    private static Queue<Unit> currentQueue = new Queue<Unit>();
    
    public static void recalculateInitiativeOrder()
    {
        activeUnits = activeUnits.Where(unit => unit != null).OrderBy(unit => unit.InGamePlay ? unit.Initiative : 1000).ToList();
        currentQueue.Clear();
        activeUnits.ForEach(unit =>
        {
            unit.CurrentTurn = false;
            currentQueue.Enqueue(unit);
        });
        currentQueue.First().CurrentTurn = true;
    }

    public static void registerUnit( Unit u )
    {
        activeUnits.Add(u);
    }

    public static void clearList()
    {
        activeUnits.Clear();
        recalculateInitiativeOrder();
    }

    public static void nextTurn()
    {
        currentQueue.First().CurrentTurn = false;
        do
        {
            currentQueue.Enqueue(currentQueue.Dequeue());
        } while (currentQueue.First().InGamePlay);

        currentQueue.First().CurrentTurn = true;
    }
}
