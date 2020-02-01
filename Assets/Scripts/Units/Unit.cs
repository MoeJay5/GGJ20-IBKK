using UnityEngine;

public class Unit : MonoBehaviour
{
    /* Variables */

    [Header("Unit Stats")]
    [SerializeField] private int health = 5;
    [SerializeField, Range(1, 100)] private int initiative = 50;
}
