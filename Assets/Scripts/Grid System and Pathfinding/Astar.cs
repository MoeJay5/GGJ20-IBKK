using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;

public class Astar
{
    private class AStarNode
    {
        public SimpleNode node;
        public float g;
        public float h;
        public AStarNode parent;

        public AStarNode(SimpleNode node, SimpleNode end)
        {
            this.node = node;

            float xDelta = node.transform.position.x - end.transform.position.x;
            float zDelta = node.transform.position.z - end.transform.position.z;

            h = Mathf.Sqrt(xDelta * xDelta + zDelta * zDelta);
        }
    };

    private class AStarCalculation
    {
        private AStarNode startNode;
        private AStarNode endNode;
        
        private Dictionary<SimpleNode, AStarNode> nodeMappings = new Dictionary<SimpleNode, AStarNode>();
        HashSet<AStarNode> open = new HashSet<AStarNode>();
        HashSet<AStarNode> closed = new HashSet<AStarNode>();

        public AStarCalculation(SimpleNode start, SimpleNode end)
        {
            endNode = new AStarNode(end, end);
            nodeMappings.Add(end, endNode);

            startNode = getAStarNode(start);
            open.Add(startNode);
        }

        AStarNode getAStarNode(SimpleNode node)
        {
            if (!nodeMappings.ContainsKey(node))
            {
                nodeMappings[node] = new AStarNode(node, endNode.node);
            }
            
            return nodeMappings[node];
        }

        List<AStarNode> getNeighboringNodes(AStarNode node)
        {
            List<AStarNode> nodes = new List<AStarNode>();
            if(node.node.UpNode != null) nodes.Add(getAStarNode(node.node.UpNode));
            if(node.node.DownNode != null) nodes.Add(getAStarNode(node.node.DownNode));
            if(node.node.LeftNode != null) nodes.Add(getAStarNode(node.node.LeftNode));
            if(node.node.RightNode != null) nodes.Add(getAStarNode(node.node.RightNode));
            return nodes;
        }

        bool ProcessStep()
        {
            if (open.Count == 0) return false;
            
            AStarNode current = open.OrderBy(n => n.g + n.h).First();
            open.Remove(current);
            
            if (current == endNode) return false;

            List<AStarNode> neighbors = getNeighboringNodes(current);

            foreach (AStarNode node in neighbors)
            {
                if (open.Contains(node) || closed.Contains(node))
                {
                    if (node.g > current.g + 1)
                    {
                        node.g = current.g + 1;
                        node.parent = current;
                    }
                }
                else
                {
                    node.g = current.g + 1;
                    node.parent = current;
                }

                if (!closed.Contains(node) && !open.Contains(node))
                {
                    open.Add(node);
                }
            }

            closed.Add(current);
            return true;
        }
        
        public List<SimpleNode> GeneratePath()
        {
            if(startNode == endNode)
            {
                List<SimpleNode> p = new List<SimpleNode>();
                p.Add(startNode.node);
                return p;
            }
            
            while (ProcessStep()) ;
            if (endNode.parent == null)
            {
                return null;
            }

            List<SimpleNode> path = new List<SimpleNode>();
            AStarNode current = endNode;
            while (current != startNode)
            {
                path.Add(current.node);
                current = current.parent;
            }

            path.Add(startNode.node);

            path.Reverse();

            return path;
        }
    }

    public static List<SimpleNode> CalculatePath(SimpleNode start, SimpleNode end)
    {
        return (new AStarCalculation(start, end)).GeneratePath();
    }
}