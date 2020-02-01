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

            var mesh =n.GetComponent<MeshRenderer>();

            if (!n.walkable)
            {
                mesh.material.color = Color.red;
            }
            else
                mesh.material.color = ogColor;

        }
    }
    private void OnSceneGUI()
    {

    }
}
