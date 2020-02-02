using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    /* Variables */

    public static Node current_UnitNode;
    [HideInInspector] public Animator anim;
    [Header ("Unit Stats")]
    [SerializeField] private int health = 5;

    public readonly float speed = 6;

    public bool InGamePlay = true;
    public bool CurrentTurn = false;

    public int initiative = -1;

    [SerializeField] int maxAP = 5;

    public bool IsEnemy = true;

    public Sprite unitIcon;

    public int previousAP = 0;

    public int MaxAP
    {
        get => maxAP;
    }

    [SerializeField] private int _AP;

    public int AP
    {
        get => _AP;
    }

    public void SetAP (int newAP)
    {
        _AP = newAP;
    }

    /* Main Functions */
    public void Awake ()
    {
        initiative = Random.Range (1, 100);
        _AP = MaxAP;
        InitiativeSystem.registerUnit (this);
    }

    public Node GetMyGridNode ()
    {
        // This should only be called if we don't know the current node. We should be able to use the last tile of the path.
        RaycastHit hit;

        if (!Physics.Raycast (transform.position + Vector3.up * 2, Vector3.down, out hit, 7, 1 << GameMaster.Layer_GridNode))
            return null;
        else
            return hit.collider.gameObject.GetComponent<Node> ();
    }

    public void DecreaseAPBy(int amount)
    {
        _AP -= amount;
    }
    public void Damage(int amount)
    {
        health -= amount;
        anim.SetTrigger("Hit");
    }
}