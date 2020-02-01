using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Singleton
    public static GameStateManager Instance;
    private void OnEnable() => Instance = this;
    #endregion

    /* Temp Dev work: Move one unit by clicking on a node */

    [Header("Placeholder until Initiative and Multiple Enemies implemented")]
    [SerializeField] private Unit currentlyInitiatedUnit;

    [Header("Dependencies")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private GridSystem generatedGrid;

    // States
    [SerializeField] private bool movingInitiatedUnit = true;

    private void Update()
    {
        if (movingInitiatedUnit == false)
            return;

        Node startingNode = currentlyInitiatedUnit.GetMyGridNode();
        Node destinationNode = GetMousedOverNode();
        if (GetMousedOverNode() == null)
            return;

        Path movementPath = Astar.CalculatePath(startingNode, destinationNode, generatedGrid);

        Debug.Log("Calculated Path!");
    }

    /* Helper Functions */

    private Node GetMousedOverNode()
    {
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.GetComponent<Node>();
    }
}
