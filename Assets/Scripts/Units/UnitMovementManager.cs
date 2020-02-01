using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof (Unit))]
public class UnitMovementManager : MonoBehaviour
{
    /* Variables */

    [Header ("Properties")]
    [SerializeField] private bool movingInitiatedUnit;

    private Unit currentlyInitiatedUnit;

    /* Main Functions */

    void OnEnable ()
    {
        currentlyInitiatedUnit = gameObject.GetComponent<Unit> ();
    }

    private void Update ()
    {

        if (movingInitiatedUnit == false)
            return;

        Node startingNode = currentlyInitiatedUnit.GetMyGridNode ();
        Node destinationNode = GetMousedOverNode ();
        if (GetMousedOverNode () == null)
            return;

        //Will calculate path AND show the path in-game
        Path movementPath = Astar.CalculatePath (startingNode, destinationNode, LevelStateManager.Instance.generatedGrid);

        if (InputListener.Instance.PressedDown_Mouse_LeftClick)
            OrderUnitMovement (currentlyInitiatedUnit, movementPath);

    }

    //Helper Functions 

    private Node GetMousedOverNode ()
    {
        RaycastHit hit;
        Ray ray = LevelStateManager.Instance.gameCamera.ScreenPointToRay (Input.mousePosition);
        if (!Physics.Raycast (ray, out hit, 100, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.GetComponent<Node> ();
    }

    private void OrderUnitMovement (Unit unitToMove, Path movementPath)
    {
        movingInitiatedUnit = false;
        StartCoroutine (MoveUnitAlongPath (unitToMove, movementPath));
    }
    private IEnumerator MoveUnitAlongPath (Unit unitToMove, Path movementPath)
    {
        Node prevNode = unitToMove.GetMyGridNode ();
        foreach (Node nextNode in Enumerable.Reverse (movementPath.nodes))
        {
            //Move Unit to Node
            float oneNodeMovementDuration = 2f;
            float t = 0;
            float tickDuration = currentlyInitiatedUnit.speed * Time.deltaTime;
            while (t < oneNodeMovementDuration)
            {
                Vector3 newPos = Vector3.Lerp (prevNode.transform.position, nextNode.transform.position, t);
                newPos.y = unitToMove.transform.position.y;
                unitToMove.transform.position = newPos;

                t += tickDuration;
                yield return null;
            }

            prevNode = nextNode;
            yield return new WaitUntil (() => unitToMove.GetMyGridNode () == nextNode);
        }

        //Done moving along path
        movingInitiatedUnit = true;
    }
}