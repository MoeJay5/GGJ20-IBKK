using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAstar : MonoBehaviour
{
    public GridSystem grid;
    public Node start;
    public Node end;
    private void Start()
    {
        
    }

    [ContextMenu("Test Path Finding")]
    void TestPath()
    {

        Path p = Astar.CalculatePath(start, end, grid);
        foreach (Node n in p.nodes)
        {
            var mesh = n.GetComponent<MeshRenderer>();
            mesh.material.color = Color.green;
        }
    }


}
