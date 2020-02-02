using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{

    public static Path CalculatePath(Node start, Node end, GridSystem grid)
    {
        HashSet<Node> open = new HashSet<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        List<Node> openList = new List<Node>();
        Node current;
        start.hScore = 0;
        start.gScore = 0;
        start.fScore = start.gScore + start.hScore;
        open.Add(start);
        openList.Add(start);
        //open.Sort((a, b) => a.fScore.CompareTo(b.fScore));  

        while (open.Count != 0)
        {
            openList.Sort((a, b) => a.fScore.CompareTo(b.fScore));
            current = openList[0];

            open.Remove(current);
            openList.Remove(current);
            closed.Add(current);
            if (current == end)
            {
                return Retrace(start, current);
            }
            //current.GetNeighbors(grid);
            foreach (Node node in current.neighbors)
            {
                if (closed.Contains(node) || !node.walkable||node.occupyingUnit!=null)
                    continue;
                var tentative_g = current.gScore + Distance(current, node);
                Node shared1 = null;
                Node shared2 = null;
                foreach (Node n in current.neighbors)
                {
                    //if (node.neighbors.Contains(n))
                    //{
                    //    if (shared1 == null)
                    //        shared1 = n;
                    //    else
                    //        shared2 = n;
                    //}

                }

                if (!open.Contains(node))
                {
                    //if (shared1 != null && shared2 != null)
                    //{
                    //    if ((shared1.walkable || shared2.walkable))
                    //    {
                    //        open.Add(node);
                    //        openList.Add(node);
                    //    }
                    //
                    //}
                    //else
                    //{
                        open.Add(node);
                        openList.Add(node);
                    //}
                }
                else if (tentative_g >= node.gScore)
                    continue;
                node.parent = current;
                node.gScore = tentative_g;
                node.hScore = Manhattan(node, end);
                node.fScore = node.gScore + node.hScore;
                //path.nodes.Add(curr)


            }
        }

        return null;
    }


    static float Distance(Node first, Node second)
    {
        if (first.transform.position.x == second.transform.position.x || first.transform.position.z == second.transform.position.z)
        {
            return 10f;

        }
        else
            return 10000f;
    }
    static float Manhattan(Node testingNode, Node destination)
    {
        return (Mathf.Abs(destination.transform.position.x - testingNode.transform.position.x) + Mathf.Abs(destination.transform.position.z - testingNode.transform.position.z)) * 10f;
    }

    static Path Retrace(Node start, Node end)
    {
        Path p = new Path();
        Node cur = end;
        while (cur != start)
        {
            p.nodes.Add(cur);
            cur = cur.parent;
        }
        return p;
    }
}



public class Path
{
    public List<Node> nodes = new List<Node>();


}


