using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public List<Node> gridNodes = new List<Node>();
    public int width;
    public int height;

    [ContextMenu("Regen Neighbors")]
    public void GenerateNeighbors()
    {
        foreach(Node n in gridNodes)
        {
            n.GetNeighbors(this);
        }
    }


}
