using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.UI;

[RequireComponent(typeof(Unit))]
public class PatrolState : State
{
    [Header("Settings")] [SerializeField] private State WaypointState;
    [SerializeField] private State NodeStepState;

    [SerializeField] private List<SimpleNode> RouteWaypoints;

    [Header("Debug Info")] 
    [SerializeField] private int routeIndex = 0;
    [SerializeField] private int pathIndex = 0;
    [SerializeField] private List<SimpleNode> currentPath = null;

    private float nodeWalkStart = -100;
    private Unit unit;

    private void OnValidate()
    {
        if (RouteWaypoints.Count == 0) return;

        unit = gameObject.GetComponent<Unit>();
        
        // Jump the unit the the first route index.
        unit.transform.position = RouteWaypoints[0].transform.position;
        unit.currentNode = RouteWaypoints[0];
    }

    public override void EnterState(StateMachine parent)
    {
        CheckPreconditions(parent);
        
        // Jump the unit the the first route index.
        unit.transform.position = RouteWaypoints[0].transform.position;
        unit.currentNode = RouteWaypoints[0];
        
        // Clear the path.
        currentPath = null;
    }

    private void CheckPreconditions(StateMachine parent)
    {
        bool failedCheck = false;
        
        if (unit == null)
        {
            Debug.LogError("Patrol State must be assigned to a Unit.");
            failedCheck = true;
        } 
        
        if (RouteWaypoints.Count < 2)
        {
            Debug.LogError("Patrol State must have a route of at least 2 nodes.");
            failedCheck = true;
        }

        if (failedCheck)
        {
            parent.PopState();
        }
    }

    public override void UpdateState(StateMachine parent)
    {
        float positionDelta = (Time.time - nodeWalkStart) * unit.currentSpeed;
        
        if (positionDelta > 1f)
        {
            Debug.Log("Setup Turn");
            SetupTurn(parent);
        }
        else
        {
            Debug.Log("Do Walk");
            DoWalk();
        }
    }

    private void SetupTurn(StateMachine parent)
    {
        // Update Route
        if (currentPath == null)
        {
            Debug.Log("Updating Route");
            unit.currentNode = RouteWaypoints[routeIndex];
            unit.transform.position = RouteWaypoints[routeIndex].transform.position;
            routeIndex = ++routeIndex % RouteWaypoints.Count;
            
            currentPath = Astar.CalculatePath(unit.currentNode, RouteWaypoints[routeIndex]);
            pathIndex = 0;
        }

        // End the path.
        if (pathIndex >= currentPath.Count)
        {
            Debug.Log("End of Path");
            currentPath = null;
            if(WaypointState != null) parent.PushState(WaypointState);
            return;
        } 
        
        // Set new target
        if (unit.currentNode == currentPath[pathIndex])
        {
            Debug.Log("Starting to Walk");
            if (unit.TryUseAP(1))
            {
                pathIndex++;
                nodeWalkStart = Time.time;
            }
            else
            {
                InitiativeSystem.nextTurn();
            }
        }
        else // Finish last walk
        {
            Debug.Log("Ending Walk");
            unit.currentNode = currentPath[pathIndex];
            unit.transform.position = currentPath[pathIndex].transform.position;
            if(NodeStepState != null) parent.PushState(NodeStepState);
        }
    }

    private void DoWalk()
    {
        Vector3 startPosition = unit.currentNode.transform.position;
        Vector3 targetPosition = currentPath[pathIndex].transform.position;

        float positionDelta = (Time.time - nodeWalkStart) * unit.currentSpeed;

        unit.transform.position = Vector3.Lerp(startPosition, targetPosition, positionDelta);
        unit.transform.LookAt(targetPosition);
    }
}