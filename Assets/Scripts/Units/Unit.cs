using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    /* Variables */

    public static Node current_UnitNode;
    public Animator anim;
    [Header ("Unit Stats")]
    public int health = 5;

    public readonly float speed = 6;

    public bool InGamePlay = true;
    public bool CurrentTurn = false;
    public bool PreparingForTurn = false;

    public int initiative = -1;

    [SerializeField] int maxAP = 5;

    public bool IsEnemy = true;

    public Sprite unitIcon;
    public Sprite unitIconDisabled;

    public int previousAP = 0;

    [SerializeField] public bool isPlayer = false;

    public int MaxAP
    {
        get => maxAP;
    }

    [SerializeField] private int _AP;

    public int AP
    {
        get => _AP;
    }

    public void SetAP(int newAP)
    {
        _AP = newAP;
    }

    /* Main Functions */
    public void Awake()
    {
        initiative = Random.Range(1, 100);
        _AP = MaxAP;
        InitiativeSystem.registerUnit(this);
    }

    public Node GetMyGridNode()
    {
        // This should only be called if we don't know the current node. We should be able to use the last tile of the path.
        RaycastHit hit;

        if (!Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 7, 1 << GameMaster.Layer_GridNode))
            return null;
        else
        {

            var n = hit.collider.gameObject.GetComponent<Node>();
            n.occupyingUnit = this;
            return n;
        }
    }

    public void DecreaseAPBy(int amount)
    {
        _AP -= amount;
    }
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (this.IsEnemy)
                LevelObjectiveSystem.Instance.ObjectiveCompleted();
            anim.SetTrigger("Defeated");
            this.InGamePlay = false;
            StartCoroutine( waitToKill());
            if(isPlayer)
                UiManager.Instance.GameOver();
        }
        else
            anim.SetTrigger("Hit");
    }

    private IEnumerator waitToKill()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        
    }
}