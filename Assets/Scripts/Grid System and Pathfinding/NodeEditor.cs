using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects()]
[CustomEditor(typeof(Node))]
public class NodeEditor : UnityEditor.Editor
{
    Color ogColor = Color.white;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        foreach (Object obj in targets)
        {
            var n = obj as Node;

            if (n == null) continue;

            var mesh = n.GetComponent<MeshRenderer>();

            if (!n.walkable)
            {
                mesh.material.SetColor("_BaseColor", Color.red);
            }
            else if (n.isStairs)
            {
                mesh.material.color = Color.blue;
                n.transform.localScale = (n.direction) ? new Vector3(1, .5f, .5f) : new Vector3(.5f, .5f, 1);
           
            }
            else
                mesh.material.color = ogColor;

            foreach (var neighbor in n.neighbors)
            {
                Debug.DrawLine(n.transform.position + Vector3.up, neighbor.transform.position + Vector3.up);
            }

        }
    }
    private void OnSceneGUI()
    {

    }
}
