using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum NodeType
{
    Stairs,
    Ground,
    Extraction,
    Inactive
}

[ExecuteInEditMode]
public class SimpleNode : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] public NodeType nodeType = NodeType.Ground;
    [SerializeField] public bool direction;
    [SerializeField] public int layer = 0;
    
    [Header("Links")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Tile stairTile;
    [SerializeField] private Tile groundTile;
    [SerializeField] private  Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    [Header("Info")] 
    [SerializeField] Unit currentUnit;

    [SerializeField] public int gridIndex;
    [SerializeField] public int gridXCoordinate;
    [SerializeField] public int gridZCoordinate;

    [SerializeField] public SimpleNode UpNode;
    [SerializeField] public SimpleNode DownNode;
    [SerializeField] public SimpleNode LeftNode;
    [SerializeField] public SimpleNode RightNode;

    [HideInInspector] public Tile tile => (nodeType == NodeType.Stairs) ? stairTile : groundTile;

    private void OnValidate()
    {
        switch (nodeType)
        {
            case NodeType.Stairs:
                meshRenderer.material = activeMaterial;
                groundTile.gameObject.SetActive(false);
                stairTile.gameObject.SetActive(true);
                stairTile.transform.localRotation = Quaternion.Euler(45,direction ? 90 : 0,90);
                break;
            case NodeType.Ground:
                meshRenderer.material = activeMaterial;
                groundTile.gameObject.SetActive(true);
                stairTile.gameObject.SetActive(false);
                break;
            case NodeType.Extraction:
                meshRenderer.material = activeMaterial;
                groundTile.gameObject.SetActive(true);
                stairTile.gameObject.SetActive(false);
                break;
            case NodeType.Inactive:
                meshRenderer.material = inactiveMaterial;
                groundTile.gameObject.SetActive(false);
                stairTile.gameObject.SetActive(false);
                break;
        }
        
        transform.localPosition = new Vector3(transform.localPosition.x, layer, transform.localPosition.z);
    }
}