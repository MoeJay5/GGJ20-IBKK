using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node : MonoBehaviour
{

    public bool walkable;
    [HideInInspector]
    public Node parent;
    [HideInInspector]
    public List<Node> neighbors = new List<Node>();
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
    public void GetNeighbors(GridSystem grid)
    {
        neighbors.Clear();
        //right
        if ((myGridIndex + 1) % grid.width != 0)
        {
            Node right = grid.gridNodes[myGridIndex + 1];
            if (right.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (right.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!right.isStairs || (right.isStairs && right.direction))
                        if (!this.isStairs || this.direction)
                            neighbors.Add(right);
            //upright
           // if (myGridIndex < grid.width * (grid.height - 1) && !isStairs)
           // {
           //     Node n = grid.gridNodes[myGridIndex + grid.width + 1];
           //     if (!n.isStairs && !this.isStairs)
           //         if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
           //             if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
           //                 neighbors.Add(n);
           // }
           // //downright
           // if (myGridIndex > (grid.width - 1) && !isStairs)
           // {
           //     Node n = grid.gridNodes[myGridIndex - grid.width + 1];
           //     if (!n.isStairs && !this.isStairs)
           //         if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
           //             if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
           //                 neighbors.Add(n);
           // }
        }
        //left
        if ((myGridIndex % grid.width) != 0)
        {
            Node left = grid.gridNodes[myGridIndex - 1];
            if (left.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (left.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!left.isStairs || (left.isStairs && left.direction))
                        if (!this.isStairs || this.direction)
                            neighbors.Add(left);
            //upleft
            //if (myGridIndex < grid.width * (grid.height - 1) && !isStairs)
            //{
            //    Node n = grid.gridNodes[myGridIndex + grid.width - 1];
            //    if (!n.isStairs && !this.isStairs)
            //        if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
            //            if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
            //                neighbors.Add(n);
            //}
            ////downleft
            //if (myGridIndex > (grid.width - 1) && !isStairs)
            //{
            //    Node n = grid.gridNodes[myGridIndex - grid.width - 1];
            //    if (!n.isStairs && !this.isStairs)
            //        if (n.transform.position.y < (this.transform.position.y + this.transform.localScale.y))
            //            if (n.transform.position.y > (this.transform.position.y - this.transform.localScale.y))
            //                neighbors.Add(n);
            //}
        }
        //up
        if (myGridIndex < grid.width * (grid.height - 1))
        {
            Node up = grid.gridNodes[myGridIndex + grid.width];
            if (up.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (up.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!up.isStairs || (up.isStairs && !up.direction))
                        if (!this.isStairs || !this.direction)
                            neighbors.Add(up);
        }
        //down
        if (myGridIndex > (grid.width - 1))
        {
            Node down = grid.gridNodes[myGridIndex - grid.width];
            if (down.transform.position.y < (this.transform.position.y + ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                if (down.transform.position.y > (this.transform.position.y - ((!this.isStairs) ? this.transform.localScale.y : this.transform.localScale.y * 4)))
                    if (!down.isStairs || (down.isStairs && !down.direction))
                        if (!this.isStairs || !this.direction)
                            neighbors.Add(down);
        }

    }

    private void OnMouseEnter()
    {
        var grid = FindObjectOfType<GridSystem>();

        Path p = Astar.CalculatePath(grid.gridNodes[0], this, grid);
        foreach (Node n in grid.gridNodes)
        {
            if (n.walkable)
                n.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
        }
        foreach (Node n in p.nodes)
        {
            var mesh = n.GetComponent<MeshRenderer>();
            mesh.material.SetColor("_BaseColor", Color.green);
        }
        //p.nodes[0].GetComponent<MeshRenderer>().material.color = Color.green;

    }
}
