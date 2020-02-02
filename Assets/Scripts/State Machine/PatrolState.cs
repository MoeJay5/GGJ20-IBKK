using System.Collections.Generic;
using UnityEngine;

class PatrolState : State
{
    [SerializeField] private List<Node> destinations;

    private Unit myUnit;
    private Path myPath;
    private int targetIndex;
    private int pathIndex;

    private Node prevNode;

    private bool playerWithinRange;

    [SerializeField] private Card_ScriptableObject card;

    public override void EnterState (StateMachine parent)
    {
        myUnit = parent.GetComponent<Unit> ();
        if (myUnit == null)
        {
            Debug.LogWarning ("Patrol assigned to state machine without unit.");
        }

        myUnit.transform.position = destinations[0].transform.position + Vector3.up;
        destinations[0].occupyingUnit = myUnit;
        prevNode = null;

        targetIndex = 1;
    }

    public override void UpdateState (StateMachine parent)
    {
        if (!myUnit.CurrentTurn)
        {
            return;
        }

        if (myPath == null)
        {
            myPath = Astar.CalculatePath (destinations[targetIndex],
                destinations[(targetIndex + 1) % destinations.Count], LevelStateManager.Instance.generatedGrid);
            pathIndex = 0;
        }

        foreach (Card_ScriptableObject.PatternNode pn in card.pattern)
        {
            var occupyingUnit = myUnit.GetMyGridNode ().GetNeighbor (PatternNodeDirection (pn)).occupyingUnit;
            playerWithinRange = occupyingUnit != null ? true : false;
            if (playerWithinRange)
            {
                Debug.Log (occupyingUnit.name + " is within range.", gameObject);
                parent.PushState (parent.gameObject.GetComponent<AttackState> ());
            }
            else Debug.Log (occupyingUnit.name + " is not within range.", gameObject);
        }

        if (!playerWithinRange)
        {
            Vector3 nextDestinationVector = myPath.nodes[pathIndex].transform.position + Vector3.up;

            if (Vector3.Distance (parent.transform.position, nextDestinationVector) < 0.001)
            {
                if (++pathIndex == myPath.nodes.Count)
                {
                    myPath = null;
                    targetIndex = (targetIndex + 1) % destinations.Count;
                }
                InitiativeSystem.nextTurn ();
                return;
            }

            parent.transform.position = Vector3.MoveTowards (parent.transform.position, nextDestinationVector,
                myUnit.speed * Time.deltaTime);
            myPath.nodes[pathIndex].occupyingUnit = myUnit;

            if (prevNode != null)
                prevNode.occupyingUnit = null;

            prevNode = myPath.nodes[pathIndex];
        }

    }

    private Direction PatternNodeDirection (Card_ScriptableObject.PatternNode pn)
    {
        if (pn.xAxis == 0 && pn.yAxis == 1)
            return Direction.Up;
        else if (pn.xAxis == 1 && pn.yAxis == 1)
            return Direction.UpRight;
        else if (pn.xAxis == 1 && pn.yAxis == 0)
            return Direction.Right;
        else if (pn.xAxis == 1 && pn.yAxis == -1)
            return Direction.DownRight;
        else if (pn.xAxis == 0 && pn.yAxis == -1)
            return Direction.Down;
        else if (pn.xAxis == -1 && pn.yAxis == -1)
            return Direction.DownLeft;
        else if (pn.xAxis == -1 && pn.yAxis == 0)
            return Direction.Left;
        else
            return Direction.UpLeft;
    }
}