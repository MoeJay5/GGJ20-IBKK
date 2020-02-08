using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class Unit : MonoBehaviour, ITurnListener
{
    /* Variables */

    [Header ("References")]
    public Animator anim;

    [Header ("Unit Description")]
    public Sprite unitIcon;
    public Sprite unitIconDisabled;
    public bool IsHero = false;
    public bool IsPlayable = false;
    public bool IsRecruitable = false;
    
    [Header ("Unit Stats")]
    public int health = 5;
    public float speed = 6;
    public int ap = 5;
    
    [Header("Initiative Stats")]
    public bool randomInitiative = true;
    public int Initiative = -1;
    public bool InGamePlay = true;
    public bool IsCurrentTurn = false;

    [Header("In Flight Stats")] 
    public int currentHealth;
    public float currentSpeed;
    public int currentAP;
    public SimpleNode currentNode;



    /* Main Functions */
    public void Awake()
    {
        if (randomInitiative)
        {
            Initiative = Random.Range(1, 100);
        }
        InitiativeSystem.registerUnit(this);

        currentHealth = health;
        currentSpeed = speed;
        currentAP = ap;
    }

    public bool TryUseAP(int amount)
    {
        if (currentAP >= amount)
        {
            currentAP -= amount;
            return true;
        }

        return false;
    }
    
    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            InGamePlay = false;
            anim.SetTrigger("Defeated");

            if (IsHero)
            {
                Debug.Log("You lose!");
            }
        }
        else
        {
            anim.SetTrigger("Hit");
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > health) currentHealth = health;
    }

    public void NextTurn(Unit unit)
    {
        if (unit == this && InGamePlay)
        {
            currentAP = ap;
        }
    }
}