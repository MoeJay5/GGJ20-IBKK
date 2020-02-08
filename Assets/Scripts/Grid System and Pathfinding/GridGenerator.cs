using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int xSize = 10;
    public int zSize = 10;
    public GameObject GridNodePrefab;

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        GameObject gridParent = new GameObject();
        gridParent.name = "Generated Grid";
        
        GridSystem grid = gridParent.AddComponent<GridSystem>();
        grid.width = xSize;
        grid.height = zSize;
        
        for (int xIndex = 0; xIndex < grid.width; xIndex++)
        {
            for (int zIndex = 0; zIndex < grid.height; zIndex++)
            {
                GameObject g = PrefabUtility.InstantiatePrefab(GridNodePrefab) as GameObject;
                g.transform.parent = gridParent.transform;
                g.transform.localPosition = new Vector3(xIndex, 0, zIndex);
                
                SimpleNode node = g.GetComponent<SimpleNode>();
                node.gridXCoordinate = xIndex;
                node.gridZCoordinate = zIndex;
                
                grid.gridNodes.Add(node);
                
                node.gridIndex = grid.gridNodes.Count;
            }
        }
    }
}
