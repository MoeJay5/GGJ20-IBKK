using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public List<SimpleNode> gridNodes = new List<SimpleNode>();
    public int width;
    public int height;

    private int lastCalculatedFrame = 0;
    private SimpleNode lastHitNode = null;
    public void Start()
    {
        CalculateNeighbors();
    }

    public void Update()
    {
        lastCalculatedFrame = Time.frameCount;
    }

    public SimpleNode getCurrentMouseNode(Camera camera)
    {
        if (lastCalculatedFrame != Time.frameCount)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit, 10000f, ~LayerMask.NameToLayer("GridNode")))
            {
                lastHitNode = hit.collider.GetComponent<SimpleNode>();
            }
        }
        return lastHitNode;
    }

    [ContextMenu("Calculate Neighbors")]
    public void CalculateNeighbors()
    {
        foreach ( SimpleNode node in gridNodes)
        {
            node.UpNode = null;
            node.DownNode = null;
            node.LeftNode = null;
            node.RightNode = null;
        }
        
        for (int index = 0; index < gridNodes.Count; index++)
        {
            SimpleNode node = gridNodes[index];

            if (node.nodeType == NodeType.Inactive) continue;

            // Right
            SimpleNode rightNode = null;
            if (node.gridXCoordinate != width - 1)
            {
                rightNode = gridNodes[index + height]; 
            }
            
            SimpleNode upNode = null;
            // Up
            if (node.gridZCoordinate != height - 1)
            {
                upNode = gridNodes[index + 1]; 
            }
            
            // Normal or Stairs
            if (node.nodeType == NodeType.Stairs)
            {
                if (node.direction)
                {

                    // Left
                    SimpleNode leftNode = null;
                    if (node.gridXCoordinate != 0)
                    {
                        leftNode = gridNodes[index - height]; 
                    }

                    node.RightNode = rightNode;
                    node.LeftNode = leftNode;

                    rightNode.LeftNode = node;
                    leftNode.RightNode = node;

                }
                else
                {
            
                    SimpleNode downNode = null;
                    // Down
                    if (node.gridZCoordinate != 0)
                    {
                        downNode = gridNodes[index - 1]; 
                    }

                    node.DownNode = downNode;
                    node.UpNode = upNode;

                    downNode.UpNode = node;
                    upNode.DownNode = node;

                }
                
            }
            else
            {
                if (
                    rightNode != null && 
                    rightNode.nodeType != NodeType.Stairs && 
                    rightNode.nodeType != NodeType.Inactive &&
                    rightNode.layer == node.layer)
                {
                    node.RightNode = rightNode;
                    rightNode.LeftNode = node;
                }
                    
                if (
                    upNode != null &&
                    upNode.nodeType != NodeType.Stairs &&
                    upNode.nodeType != NodeType.Inactive &&
                    upNode.layer == node.layer)
                {
                    node.UpNode = upNode;
                    upNode.DownNode = node;
                }
            }
        }
    }
    
    [ContextMenu("Show Grid")]
    public void AddGridHighlighter()
    {
        foreach(SimpleNode n in gridNodes)
        {
            MeshRenderer mesh = n.gameObject.GetComponent<MeshRenderer>();
            if(mesh != null) mesh.enabled = true;

            GridHighlighter gh = n.gameObject.GetComponent<GridHighlighter>();
            if (gh != null) gh.enabled = true;
        }
    }
    
    [ContextMenu("Hide Grid")]
    public void HideGrid()
    {
        foreach(SimpleNode n in gridNodes)
        {
            MeshRenderer mesh = n.gameObject.GetComponent<MeshRenderer>();
            if(mesh != null) mesh.enabled = false;

            GridHighlighter gh = n.gameObject.GetComponent<GridHighlighter>();
            if (gh != null) gh.enabled = false;
        }
    }
}
