using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft,
}

public class Node : MonoBehaviour
{
    public static Node current_SelectedNode;

    public Unit occupyingUnit = null;
    [SerializeField] public Tile tile;

    public bool walkable;
    [HideInInspector]
    public Node parent;
    [HideInInspector]
    public List<Node> neighbors = new List<Node> ();
    [HideInInspector]
    public Dictionary<Direction, Node> neighborDirections = new Dictionary<Direction, Node> ();
    [SerializeField]
    private List<Direction> directionsList = new List<Direction>();
    [SerializeField]
    private List<Node> nodeList = new List<Node>();
    [HideInInspector]
    public float gScore;
    [HideInInspector]
    public float hScore;
    [HideInInspector]
    public float fScore;
    [HideInInspector]
    public int myGridIndex;
    public bool isStairs = false;
    public bool direction = false;

    private void Start()
    {
        for(int i = 0;i<nodeList.Count;i++)
        {
            neighborDirections.Add(directionsList[i], nodeList[i]);
        }
    }

    public void CalculateNeighbors (GridSystem grid)
    {
        neighbors.Clear ();
        neighborDirections.Clear ();
        nodeList.Clear();
        directionsList.Clear();
       
        //right
        if ((myGridIndex + 1) % grid.width != 0)
        {
            Node right = grid.gridNodes[myGridIndex + 1];
            if (right.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (right.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!right.isStairs || (right.isStairs && right.direction))
                        if (!this.isStairs || this.direction)
                        {
                            neighbors.Add (right);
                            //neighborDirections.Add (Direction.Right, right);
                            directionsList.Add(Direction.Right);
                            nodeList.Add(right);
                        }
            //upright
            if (myGridIndex < grid.width * (grid.height - 1) && !isStairs)
            {
                Node n = grid.gridNodes[myGridIndex + grid.width + 1];
                if (!n.isStairs && !this.isStairs)
                    if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
                        if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
                        {
                            neighbors.Add (n);
                            // neighborDirections.Add (Direction.UpRight, n);
                            directionsList.Add(Direction.UpRight);
                            nodeList.Add(n);
                        }
            }
            //downright
            if (myGridIndex > (grid.width - 1) && !isStairs)
            {
                Node n = grid.gridNodes[myGridIndex - grid.width + 1];
                if (!n.isStairs && !this.isStairs)
                    if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
                        if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
                        {
                            neighbors.Add (n);
                            //neighborDirections.Add (Direction.DownRight, n);
                            directionsList.Add(Direction.DownRight);
                            nodeList.Add(n);
                        }
            }
        }
        //left
        if ((myGridIndex % grid.width) != 0)
        {
            Node left = grid.gridNodes[myGridIndex - 1];
            if (left.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (left.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!left.isStairs || (left.isStairs && left.direction))
                        if (!this.isStairs || this.direction)
                        {
                            neighbors.Add (left);
                            //neighborDirections.Add (Direction.Left, left);
                            directionsList.Add(Direction.Left);
                            nodeList.Add(left);
                        }
            //upleft
            if (myGridIndex < grid.width * (grid.height - 1) && !isStairs)
            {
                Node n = grid.gridNodes[myGridIndex + grid.width - 1];
                if (!n.isStairs && !this.isStairs)
                    if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
                        if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
                        {
                            neighbors.Add (n);
                            //neighborDirections.Add (Direction.UpLeft, n);
                            directionsList.Add(Direction.UpLeft);
                            nodeList.Add(n);
                        }
            }
            //downleft
            if (myGridIndex > (grid.width - 1) && !isStairs)
            {
                Node n = grid.gridNodes[myGridIndex - grid.width - 1];
                if (!n.isStairs && !this.isStairs)
                    if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
                        if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
                        {
                            neighbors.Add (n);
                            //neighborDirections.Add (Direction.DownLeft, n);
                            directionsList.Add(Direction.DownLeft);
                            nodeList.Add(n);
                        }
            }
        }
        //up
        if (myGridIndex < grid.width * (grid.height - 1))
        {
            Node up = grid.gridNodes[myGridIndex + grid.width];
            if (up.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (up.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!up.isStairs || (up.isStairs && !up.direction))
                        if (!this.isStairs || !this.direction)
                        {
                            neighbors.Add (up);
                            //neighborDirections.Add (Direction.Up, up);
                            directionsList.Add(Direction.Up);
                            nodeList.Add(up);
                        }
        }
        //down
        if (myGridIndex > (grid.width - 1))
        {
            Node down = grid.gridNodes[myGridIndex - grid.width];
            if (down.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (down.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!down.isStairs || (down.isStairs && !down.direction))
                        if (!this.isStairs || !this.direction)
                        {
                            neighbors.Add (down);
                            //neighborDirections.Add (Direction.Down, down);
                            directionsList.Add(Direction.Down);
                            nodeList.Add(down);
                        }
        }

    }

    public Node GetNeighbor (Direction dir)
    {
        Node n = null;
        neighborDirections.TryGetValue (dir, out n);
        return n;
    }

    private void OnMouseEnter ()
    {
        current_SelectedNode = this;
    }
}