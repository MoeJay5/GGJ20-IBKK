using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridHighlighter : MonoBehaviour
{
    private SimpleNode node;
    void Start()
    {
        node = gameObject.GetComponent<SimpleNode>();
    }

    void DrawNodeRay(SimpleNode target)
    {
        if (target == null) return;
        Vector3 start = transform.position + Vector3.up;
        Vector3 end = target.transform.position + Vector3.up;
        Vector3 direction = (end - start) * 0.4f;
        Debug.DrawRay(start, direction, Color.grey);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (node == null) return;
        DrawNodeRay(node.UpNode);
        DrawNodeRay(node.DownNode);
        DrawNodeRay(node.LeftNode);
        DrawNodeRay(node.RightNode);
    }
}
