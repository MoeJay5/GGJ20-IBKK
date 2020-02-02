using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

[RequireComponent (typeof (Unit))]
public class UnitMovementManager : MonoBehaviour
{
    /* Variables */

    [Header ("Properties")]
    [SerializeField] private bool movingInitiatedUnit;
    [SerializeField] private Node startingNode;

    private Unit currentlyInitiatedUnit;
    private GridSystem grid;
    private float lastClickTime_ForDoubleClick;

    private float catchTime_ForDoubleClick = 0.25f;
    private bool unitIsMoveing;

    [Header("Dependencies")]
    [SerializeField] private Animator myAnimator = null;

    /* Main Functions */

    void OnEnable ()
    {
        currentlyInitiatedUnit = gameObject.GetComponent<Unit> ();
        grid = FindObjectOfType<GridSystem> ();

        if (startingNode == null)
            startingNode = grid.gridNodes[0];
    }

    private void Update ()
    {
        if (currentlyInitiatedUnit.AP == 0 && currentlyInitiatedUnit.CurrentTurn && movingInitiatedUnit)
        {
            InitiativeSystem.nextTurn();
            return;
        }

        if (movingInitiatedUnit == false)
            return;

        Node startingNode = currentlyInitiatedUnit.GetMyGridNode ();
        Node destinationNode = GetMousedOverNode ();
        if (GetMousedOverNode () == null)
            return;

        //Will calculate path AND show the path in-game
        Path movementPath = Astar.CalculatePath (startingNode, destinationNode, LevelStateManager.Instance.generatedGrid);

        if (InputListener.Instance.PressedDown_Mouse_LeftClick)
        {
            if (Time.time - lastClickTime_ForDoubleClick < catchTime_ForDoubleClick)
            {
                OrderUnitMovement (currentlyInitiatedUnit, movementPath);
            }
            lastClickTime_ForDoubleClick = Time.time;
        }

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
            if (currentlyInitiatedUnit.AP <= 0)
                break;

            currentlyInitiatedUnit.DecreaseAPBy (1);

            myAnimator.transform.LookAt(nextNode.transform, Vector3.up);
            myAnimator.SetBool("Walking", true);

            //Move Unit to Node
            float t = 0;
            float tickDuration = currentlyInitiatedUnit.speed * Time.deltaTime;
            while (t < 1)
            {
                unitIsMoveing = true;
                Vector3 newPos = Vector3.Lerp (prevNode.transform.position, nextNode.transform.position, t);
                //newPos.y = unitToMove.transform.position.y;
                newPos.y += nextNode.transform.localScale.y / 2;
                unitToMove.transform.position = newPos;

                t += tickDuration;
                yield return null;
            }

            unitIsMoveing = false;
            Unit.current_UnitNode = nextNode;


            myAnimator.SetBool("Walking", false);

            prevNode = nextNode;
            yield return new WaitUntil (() => unitToMove.GetMyGridNode () == nextNode);
        }

        //Done moving along path
        movingInitiatedUnit = true;
    }

    private void HighlightNavigation ()
    {
        if (unitIsMoveing)
            return;

        if (Unit.current_UnitNode == null)
            Unit.current_UnitNode = startingNode;
        Path p = Astar.CalculatePath (Unit.current_UnitNode, Node.current_SelectedNode, grid);
        foreach (Node n in grid.gridNodes)
        {
            if (n.walkable)
                n.GetComponent<MeshRenderer> ().material.SetColor ("_BaseColor", Color.white);
        }

        int allowedMovement = currentlyInitiatedUnit.AP;
        foreach (Node n in Enumerable.Reverse (p.nodes))
        {
            var mesh = n.GetComponent<MeshRenderer> ();
            if (allowedMovement > 0)
                mesh.material.SetColor ("_BaseColor", Color.green);
            else
                mesh.material.SetColor ("_BaseColor", Color.red);

            allowedMovement--;
        }
        //p.nodes[0].GetComponent<MeshRenderer>().material.color = Color.green;
    }

}