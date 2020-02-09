using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.UI;

[RequireComponent(typeof(Unit))]
public class PatrolState : State
{
    [Header("Settings")] [SerializeField] private State WaypointState;
    [SerializeField] private State NodeStepState;
    [SerializeField] private AnimationCurve MovementCurve;
    [SerializeField] private float MovementDuration = 1;
    [SerializeField] private List<SimpleNode> RouteWaypoints;

    [Header("Debug Info")] 
    [SerializeField] private int routeIndex = 0;
    [SerializeField] private int pathIndex = 0;
    [SerializeField] private List<SimpleNode> currentPath = null;

    private float nodeWalkStart = -100;
    private Unit unit;
    private bool delayedWalkActionFlag = false;

    private void OnValidate()
    {
        if (RouteWaypoints.Count == 0) return;

        unit = gameObject.GetComponent<Unit>();
        
        // Jump the unit the the first route index.
        unit.transform.position = RouteWaypoints[0].transform.position;
        unit.currentNode = RouteWaypoints[0];
        unit.currentNode.tile.setState(Tile.TileStates.ACTIVE);
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
        float positionDelta = (Time.time - nodeWalkStart) / MovementDuration;
        
        if (positionDelta > 1f)
        {
            SetupTurn(parent);
        }
        else
        {
            DoWalk();
        }
    }

    private void SetupTurn(StateMachine parent)
    {
        // Update Route
        if (currentPath == null)
        {
            unit.currentNode = RouteWaypoints[routeIndex];
            unit.transform.position = RouteWaypoints[routeIndex].transform.position;
            routeIndex = ++routeIndex % RouteWaypoints.Count;

            currentPath = Astar.CalculatePath(unit.currentNode, RouteWaypoints[routeIndex]);
            if (currentPath.Count > unit.ap)
            {
                currentPath = currentPath.GetRange(0, unit.ap + 1).ToList();
            }
            pathIndex = 0;
        }

        // End the path.
        if (pathIndex >= currentPath.Count - 1)
        {
            currentPath = null;
            unit.anim.SetBool("Walking", false);
            if(WaypointState != null) parent.PushState(WaypointState);
            return;
        } 
        
        // Project
        for (int i = pathIndex + 1; i < currentPath.Count; i++)
        {
            currentPath[i].tile.setState(Tile.TileStates.PROJECT);
        }
        
        // Set new target
        if (unit.currentNode == currentPath[pathIndex])
        {
            if (unit.currentAP > 0)
            {
                pathIndex++;
                nodeWalkStart = Time.time;
                currentPath[pathIndex].tile.setState(Tile.TileStates.ACTIVE);
                delayedWalkActionFlag = true;
            }
            else
            {
                unit.anim.SetBool("Walking", false);
                for (int i = pathIndex; i < currentPath.Count; i++)
                {
                    currentPath[i].tile.setState(Tile.TileStates.INACTIVE);
                }
                
                InitiativeSystem.nextTurn();
            }
        }
        else // Finish last walk
        {
            unit.currentNode = currentPath[pathIndex];
            unit.transform.position = currentPath[pathIndex].transform.position;
            if(NodeStepState != null) parent.PushState(NodeStepState);
        }
    }

    private void DoWalk()
    {
        unit.anim.SetBool("Walking", true);
        Vector3 startPosition = unit.currentNode.transform.position;
        Vector3 targetPosition = currentPath[pathIndex].transform.position;

        float movementDelta;
        if (MovementCurve != null)
        {
            movementDelta = MovementCurve.Evaluate(Time.time - nodeWalkStart);
        }
        else
        {
            movementDelta = (Time.time - nodeWalkStart) / MovementDuration;
        }
        
        unit.transform.position = Vector3.Lerp(startPosition, targetPosition, movementDelta);
        unit.transform.LookAt(targetPosition);

        if (delayedWalkActionFlag && movementDelta > 0.75f)
        {
            delayedWalkActionFlag = false;
            unit.currentNode.tile.setState(Tile.TileStates.INACTIVE);
            unit.TryUseAP(1);
        }
    }
}