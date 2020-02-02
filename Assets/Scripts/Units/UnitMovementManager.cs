using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;
using Random = UnityEngine.Random;

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

    [Header ("Dependencies")]
    [SerializeField] private Animator myAnimator = null;

    private Unit myUnit;
    /* Main Functions */

    void OnEnable ()
    {
        currentlyInitiatedUnit = gameObject.GetComponent<Unit> ();
        grid = FindObjectOfType<GridSystem> ();

        if (startingNode == null)
            startingNode = grid.gridNodes[0];
        myUnit = this.GetComponent<Unit> ();
        myUnit.anim = myAnimator;
    }

    private void Update ()
    {
        if (!myUnit.CurrentTurn)
        {
            return;
        }
        
        if (currentlyInitiatedUnit.AP == 0 && currentlyInitiatedUnit.CurrentTurn && movingInitiatedUnit)
        {
            InitiativeSystem.nextTurn ();
            return;
        }

        if (currentlyInitiatedUnit == myUnit)
        {
            if (HandCardManager.Instance.CurrentlySelectedCard != null)
            {
                clearTiles();
                PlayerTurnHandleRotate();
                foreach (var p in HandCardManager.Instance.CurrentlySelectedCard.cardRef.pattern)
                {
                    Node n = myUnit.GetMyGridNode();

                    for (int i = 0; i < Mathf.Abs(p.xAxis); i++)
                    {
                        if (p.xAxis > 0)
                        {
                            n = n?.GetNeighbor(Direction.Right);
                        }
                        else
                            n = n?.GetNeighbor(Direction.Left);
                    }
                    for (int j = 0; j < Mathf.Abs(p.yAxis); j++)
                    {
                        if (p.yAxis > 0)
                        {
                            n = n?.GetNeighbor(Direction.Up);
                        }
                        else
                            n = n?.GetNeighbor(Direction.Down);
                    }
                    n?.tile?.setState(Tile.TileStates.ACTIVE);
                    n?.tile?.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (p.isGood) ? HandCardManager.BlueOutline : HandCardManager.RedOutline);
                }
            }
            else
            {
                clearTiles();
            }
        }
            
        if (movingInitiatedUnit == false)
            return;

        Node startingNode = currentlyInitiatedUnit.GetMyGridNode ();
        Node destinationNode = GetMousedOverNode ();
        if (GetMousedOverNode () == null)
            return;

        //Will calculate path AND show the path in-game
        Path movementPath = Astar.CalculatePath (startingNode, destinationNode, LevelStateManager.Instance.generatedGrid);

        if (movementPath != null && movementPath.nodes.Count > 0)
        {
            if (movementPath.nodes.Count > myUnit.AP)
            {
                movementPath.nodes = movementPath.nodes.GetRange (movementPath.nodes.Count - myUnit.AP, myUnit.AP);
            }

            if (movementPath.nodes.Count <= 0) return;
        
            if (movementPath.nodes.First().isStairs)
            {
                movementPath.nodes.Remove (movementPath.nodes.First ());
            }
        }

        if (InputListener.Instance.PressedDown_Mouse_LeftClick)
        {
            if (Time.time - lastClickTime_ForDoubleClick < catchTime_ForDoubleClick)
            {
                if(movementPath!=null)
                    OrderUnitMovement (currentlyInitiatedUnit, movementPath);
            }
            lastClickTime_ForDoubleClick = Time.time;
        }
        if (HandCardManager.Instance.CurrentlySelectedCard == null) 
            HighlightNavigation (movementPath);
    }

    //Helper Functions 

    private void clearTiles ()
    {
        foreach (Node n in grid.gridNodes)
        {
            if (n.walkable)
            {
                n.tile?.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", HandCardManager.WhiteOutline);
                n.tile?.setState (Tile.TileStates.INACTIVE);
            }
        }
    }

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
        if (movementPath == null) yield break;
        Node prevNode = unitToMove.GetMyGridNode ();
        foreach (Node nextNode in Enumerable.Reverse (movementPath.nodes))
        {
            if (currentlyInitiatedUnit.AP <= 0 || movementPath.nodes[0].isStairs)
                break;

            currentlyInitiatedUnit.DecreaseAPBy (1);

            myAnimator.transform.LookAt (nextNode.transform, Vector3.up);
            Vector3 localRot = myAnimator.transform.localEulerAngles;
            localRot.x = 0;
            myAnimator.transform.localEulerAngles = localRot;
            myAnimator.SetBool ("Walking", true);

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

            myAnimator.SetBool ("Walking", false);

            prevNode.tile.setState (Tile.TileStates.INACTIVE);
            prevNode = nextNode;
            yield return new WaitUntil (() => unitToMove.GetMyGridNode () == nextNode);
        }

        //Done moving along path
        movingInitiatedUnit = true;
        clearTiles ();
    }

    private void HighlightNavigation (Path p)
    {
        if (unitIsMoveing)
            return;

        if (Unit.current_UnitNode == null)
            Unit.current_UnitNode = startingNode;

        clearTiles ();

        if (p == null) return;
        foreach (Node n in Enumerable.Reverse (p.nodes))
        {
            n.tile?.setState (Tile.TileStates.ACTIVE);
        }
    }

    void PlayerTurnHandleRotate ()
    {
        var node = CardUiHandler.NodeMousedOver ();
        if (node == null)
            return;
        var myNode = currentlyInitiatedUnit.GetMyGridNode ();
        if (myNode.transform.position.y != node.transform.position.y)
            return;
        float direction = 0;
        float xdif = myNode.transform.position.x - node.transform.position.x;
        float ydif = myNode.transform.position.z - node.transform.position.z;
        if (Mathf.Abs (xdif) > Mathf.Abs (ydif))
        {
            if (xdif > 0)
                direction = 270;
            else
                direction = 90;
        }
        else if (Mathf.Abs (xdif) == Mathf.Abs (ydif))
        {
            var picker = Random.Range (0, 1);
            if (picker == 0)
                direction = (xdif > 0) ? 270 : 90;
            else
                direction = (ydif > 0) ? 180 : 0;

        }
        else
        {
            if (ydif > 0)
                direction = 180;
        }
        //
        //if (myNode.transform.position.x < node.transform.position.x)
        //{
        //    direction = 90;
        //}
        //if (myNode.transform.position.z > node.transform.position.z)
        //{
        //    direction = 180;
        //}
        //if (myNode.transform.position.x > node.transform.position.x)
        //    direction = 270;
        RotateCardPatern (direction);
        direction *= Mathf.Deg2Rad;
        // currentlyInitiatedUnit.transform.Rotate(Vector3.up, direction, Space.Self);
        StartCoroutine (RotateThisObject (direction));
    }
    IEnumerator RotateThisObject (float angle)
    {

        for (int i = 0; i < 60; i++)
        {
            currentlyInitiatedUnit.transform.rotation = Quaternion.Slerp (this.transform.rotation, new Quaternion (0, Mathf.Sin ((angle) / 2f), 0, Mathf.Cos ((angle) / 2f)), 1f / 30f);
            yield return null;
        }
    }

    void RotateCardPatern (float angle)
    {
        angle = -angle;
        //angle = angle - 90;
        for (int i = 0; i < HandCardManager.Instance.CurrentlySelectedCard.cardRef.OriginalPattern.Count; i++)
        {
            var ogPatern = HandCardManager.Instance.CurrentlySelectedCard.cardRef.OriginalPattern[i];
            HandCardManager.Instance.CurrentlySelectedCard.cardRef.pattern[i].xAxis = (int)Mathf.Round( ((ogPatern.xAxis * Mathf.Cos (angle * Mathf.Deg2Rad)) - (ogPatern.yAxis * Mathf.Sin (angle * Mathf.Deg2Rad))));
            HandCardManager.Instance.CurrentlySelectedCard.cardRef.pattern[i].yAxis = (int)Mathf.Round( ((ogPatern.xAxis * Mathf.Sin (angle * Mathf.Deg2Rad)) + (ogPatern.yAxis * Mathf.Cos (angle * Mathf.Deg2Rad))));
        }

    }

}