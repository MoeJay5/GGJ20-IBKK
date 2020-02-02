using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITurnListener
{
    void NextTurn(Unit unit);
}

public static class InitiativeSystem
{
    private static List<Unit> activeUnits = new List<Unit>();
    private static List<ITurnListener> turnListeners = new List<ITurnListener>();
    public static Queue<Unit> currentQueue = new Queue<Unit>();
    private static InitiativeUIManager uiManager;

    public static void recalculateInitiativeOrder()
    {
        activeUnits = activeUnits.Where(unit => unit != null).OrderBy(unit => unit.InGamePlay ? unit.initiative : 1000).ToList();
        currentQueue.Clear();
        activeUnits.ForEach(unit =>
        {
            unit.CurrentTurn = false;
            currentQueue.Enqueue(unit);
        });
        currentQueue.First().CurrentTurn = true;
    }

    public static void registerListener(ITurnListener l)
    {
        turnListeners.Add(l);
    }

    public static void registerManager(InitiativeUIManager ui)
    {
        uiManager = ui; 
    }

    public static Unit currentUnit()
    {
        if (currentQueue.Count > 0)
        {
            return currentQueue.Peek();
        }

        return null;
    }

    public static void registerUnit( Unit u )
    {
        activeUnits.Add(u);
        recalculateInitiativeOrder();
    }

    public static void clearList()
    {
        activeUnits.Clear();
    }

    public static void finishNextTurn()
    {
        currentQueue.Enqueue(currentQueue.Dequeue());
        Unit u = currentQueue.First();

        if (!u.InGamePlay)
        {
            nextTurn();
            return;
        }
        
        u.previousAP = u.AP;
        u.SetAP(u.MaxAP);
        currentQueue.First().CurrentTurn = true;

        turnListeners = turnListeners.Where(l => l != null).ToList();
        Unit unit = currentQueue.First();
        turnListeners.ForEach(l => l.NextTurn(unit));
    }

    public static void nextTurn()
    {
        currentQueue.First().CurrentTurn = false;
        if (uiManager != null)
        {
            uiManager.TriggerInitiativeChange();
        }
        else
        {
            finishNextTurn();
        }
    }
}
