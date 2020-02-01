using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node : MonoBehaviour
{

    public bool walkable;
    public Node parent;
    public List<Node> neighbors = new List<Node>();
    public float gScore;
    public float hScore;
    public float fScore;
    public int myGridIndex;
    public void GetNeighbors(GridSystem grid)
    {
        neighbors.Clear();
        //right
        if ((myGridIndex + 1) % grid.width != 0)
        {
            neighbors.Add(grid.gridNodes[myGridIndex + 1]);
            //upright
            if (myGridIndex < grid.width * (grid.height - 1))
                neighbors.Add(grid.gridNodes[myGridIndex + grid.width + 1]);
            //downright
            if (myGridIndex > (grid.width - 1))
                neighbors.Add(grid.gridNodes[myGridIndex - grid.width + 1]);
        }
        //left
        if ((myGridIndex % grid.width) != 0)
        {
            neighbors.Add(grid.gridNodes[myGridIndex - 1]);
            //upleft
            if (myGridIndex < grid.width * (grid.height - 1))
                neighbors.Add(grid.gridNodes[myGridIndex + grid.width - 1]);
            //downleft
            if (myGridIndex > (grid.width - 1))
                neighbors.Add(grid.gridNodes[myGridIndex - grid.width - 1]);
        }
        //up
        if (myGridIndex < grid.width * (grid.height - 1))
        {
            neighbors.Add(grid.gridNodes[myGridIndex + grid.width]);
        }
        //down
        if (myGridIndex > (grid.width - 1))
        {
            neighbors.Add(grid.gridNodes[myGridIndex - grid.width]);
        }
        //upright

    }

    private void OnMouseEnter()
    {
        var grid = FindObjectOfType<TestAstar>().grid;
        var astar = FindObjectOfType<Astar>();
        Path p = astar.AstarCalc(grid.gridNodes[0], this, grid);
        // foreach(Node n in grid.gridNodes)
        // {
        //     n.GetComponent<MeshRenderer>().material.color = Color.white;
        // }
        // foreach (Node n in p.nodes)
        // {
        //     var mesh = n.GetComponent<MeshRenderer>();
        //     mesh.material.color = Color.green;
        // }
        //p.nodes[0].GetComponent<MeshRenderer>().material.color = Color.green;

    }
}
