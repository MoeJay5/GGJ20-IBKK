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
}
