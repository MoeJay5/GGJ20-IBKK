using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTester : MonoBehaviour
{
    [SerializeField] private GridSystem grid;
    [SerializeField] private Camera camera;

    private SimpleNode lastHitNode = null;
    private SimpleNode startNode = null;
    private List<SimpleNode> lastPath = null;

    private void Start()
    {
        startNode = grid.gridNodes[0];
    }

    void UpdatePath(SimpleNode start, SimpleNode end)
    {
        if (start == null || end == null) return;
        
        if (lastPath != null)
        {
            foreach (SimpleNode n in lastPath)
            {
                n.tile.setState(Tile.TileStates.INACTIVE);
            }
        }

        lastPath = Astar.CalculatePath(start, end);
        if (lastPath != null)
        {
            foreach (SimpleNode n in lastPath)
            {
                n.tile.setState(Tile.TileStates.ACTIVE);
            }
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        SimpleNode node = grid.GetCurrentMouseNode(camera);
        if (node != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                startNode = node;
                UpdatePath(startNode, lastHitNode);
            } else if (node != lastHitNode && node != null)
            {
                UpdatePath(startNode, node);
                lastHitNode = node;
            }
        }
    }
}
