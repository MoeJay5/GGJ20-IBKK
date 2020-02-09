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
        // Clear old units
        activeUnits = activeUnits.Where(unit => unit != null).ToList();
        
        // Return if empty.
        if (activeUnits.Count == 0) return;
        
        // Sort + Update
        activeUnits = activeUnits.OrderBy(unit => unit.InGamePlay ? unit.Initiative : unit.Initiative + 1000).ToList();
        
        currentQueue.Clear();
        activeUnits.ForEach(unit =>
        {
            unit.IsCurrentTurn = false;
            currentQueue.Enqueue(unit);
        });
        currentQueue.First().IsCurrentTurn = true;
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
    
    // It is the responsibility of the UI Manager to call this function;
    public static void finishNextTurn()
    {
        currentQueue.Enqueue(currentQueue.Dequeue());
        Unit u = currentQueue.First();

        if (!u.InGamePlay)
        {
            nextTurn();
            return;
        } 
        
        // Clear old listeners
        turnListeners = turnListeners.Where(l => l != null).ToList();
        
        // Switch to new unit and notify
        Unit unit = currentQueue.First();
        unit.IsCurrentTurn = true;
        unit.StartOfTurn();
        turnListeners.ForEach(l => l.NextTurn(unit));
    }

    // It is the responsibility of the unit to call this function;
    public static void nextTurn()
    {
        if (currentQueue.Count == 0) return;
        
        currentQueue.First().IsCurrentTurn = false;
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
