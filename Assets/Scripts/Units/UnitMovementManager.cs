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

        HighlightNavigation ();

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
            float t = 0;
            float tickDuration = currentlyInitiatedUnit.speed * Time.deltaTime;
            while (t < 1)
            {
                Vector3 newPos = Vector3.Lerp (prevNode.transform.position, nextNode.transform.position, t);
                //newPos.y = unitToMove.transform.position.y;
                unitToMove.transform.position = newPos;

                t += tickDuration;
                yield return null;
            }

            Unit.current_UnitNode = nextNode;

            prevNode = nextNode;
            yield return new WaitUntil (() => unitToMove.GetMyGridNode () == nextNode);
        }

        //Done moving along path
        movingInitiatedUnit = true;
    }

    private void HighlightNavigation ()
    {
        var grid = FindObjectOfType<GridSystem> ();

        if (Unit.current_UnitNode == null)
            Unit.current_UnitNode = grid.gridNodes[0];
        Path p = Astar.CalculatePath (Unit.current_UnitNode, Node.current_SelectedNode, grid);
        foreach (Node n in grid.gridNodes)
        {
            if (n.walkable)
                n.GetComponent<MeshRenderer> ().material.SetColor ("_BaseColor", Color.white);
        }
        foreach (Node n in p.nodes)
        {
            var mesh = n.GetComponent<MeshRenderer> ();
            mesh.material.SetColor ("_BaseColor", Color.green);
        }
        //p.nodes[0].GetComponent<MeshRenderer>().material.color = Color.green;
    }

}