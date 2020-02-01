using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Singleton
    public static GameStateManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Temp Dev work: Move one unit by clicking on a node */

    [Header("Temp Dev References")]
    [SerializeField] private Camera camera;
    [SerializeField] private GridSystem generatedGrid;
    [SerializeField] private Unit currentlySelectedUnit;
    [SerializeField] private bool movingSelectedUnit = true;
    private Node currentlySelectedUnitNode;

    private void Start()
    {
        currentlySelectedUnitNode = currentlySelectedUnit.GetMyGridNode();
    }

    private void Update()
    {
        Node destination = GetMousedOverNode();
        if (GetMousedOverNode() == null)
            return;

        Path movementPath = Astar.CalculatePath(currentlySelectedUnitNode, destination, generatedGrid);

        Debug.Log("Calculated Path!");
    }

    /* Helper Functions */

    private Node GetMousedOverNode()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.GetComponent<Node>();
    }
}
