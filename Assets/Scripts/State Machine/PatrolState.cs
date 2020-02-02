using System.Collections.Generic;
using System.Linq;
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
        Debug.Log ("ENTER STATE");
        myUnit = parent.GetComponent<Unit> ();
        if (myUnit == null)
        {
            Debug.LogWarning ("Patrol assigned to state machine without unit.");
        }

        myUnit.transform.position = destinations[0].transform.position + Vector3.up;
        destinations[0].occupyingUnit = myUnit;
        // if (prevNode != null)
        //     prevNode.occupyingUnit = null;

        // prevNode = null;

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
            Node start = destinations[targetIndex];
            Node end = destinations[(targetIndex + 1) % destinations.Count];
            if (end == myUnit.GetMyGridNode ())
            {
                targetIndex = (targetIndex + 1) % destinations.Count;
                return;
            }
            myPath = Astar.CalculatePath (start, end, LevelStateManager.Instance.generatedGrid);
            pathIndex = 0;
        }

        var checkDetection = false;
        for (int i = 0; i < card.pattern.Count - 1; i++)
        {
            var node = myUnit.GetMyGridNode ().GetNeighbor (PatternNodeDirection (card.pattern[i]));
            if (node == null)
                continue;

            var unitDetected = node.occupyingUnit;
            playerWithinRange = unitDetected != null ? true : false;
            if (playerWithinRange)
            {
                if (unitDetected)
                {
                    checkDetection = true;
                    Debug.Log (unitDetected.name + " is within range.", gameObject);
                }
                if (myUnit.IsEnemy && !unitDetected.IsEnemy)
                {
                    Debug.Log (myUnit.name + " is attacking " + unitDetected.name + " With the card: " + card);
                    //parent.PushState (parent.gameObject.GetComponent<AttackState> ());
                    unitDetected.health -= card.effectIntensity;
                    checkDetection = true;
                }
            }
            else if (unitDetected) Debug.Log (unitDetected.name + " is not within range.", gameObject);
        }
        if (checkDetection)
            InitiativeSystem.nextTurn ();

        if (!playerWithinRange)
        {
            if (myPath == null)
            {
                InitiativeSystem.nextTurn ();
                return;
            }

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

            if (prevNode != null && prevNode != myUnit.GetMyGridNode ())
            {
                prevNode.occupyingUnit = null;
            }

            prevNode = myPath.nodes[pathIndex];
            //prevNode.occupyingUnit = myUnit;
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