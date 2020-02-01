using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int dimensions = 10;
    public float PhysicalNodeSize = 1f;

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        GameObject gridParent = new GameObject();
        gridParent.name = "Generated Grid";
        GridSystem grid = gridParent.AddComponent<GridSystem>();
        grid.height = dimensions;
        grid.width = dimensions;
        
        for (int i = 0; i < grid.width * grid.height; i++)
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.parent = gridParent.transform;
            g.transform.localScale *= PhysicalNodeSize;
            g.transform.localPosition = new Vector3((i % grid.width) * PhysicalNodeSize, 0, (i / grid.width) * PhysicalNodeSize);
            var node = g.AddComponent<Node>();
            node.walkable = true;
            node.myGridIndex = i;
            grid.gridNodes.Add(node);
        }

        grid.GenerateNeighbors();
    }


}
