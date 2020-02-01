using UnityEngine;

public class Unit : MonoBehaviour
{
    /* Variables */

    [Header("Unit Stats")]
    [SerializeField] private int health = 5;
    private int initiative = -1;

    /* Main Functions */

    public void SetRandomInitiative()
    {
        initiative = Random.Range(1, 100);
    }

    public Node GetMyGridNode()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 7, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.gameObject.GetComponent<Node>();
    }
}
