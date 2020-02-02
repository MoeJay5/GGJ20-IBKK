using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class Card_ScriptableObject : ScriptableObject
{
    public string cardName;
    public int cost;
    public int effectIntensity;
    public Sprite cardImage;
    public List<PatternNode> pattern = new List<PatternNode> ();
    public List<PatternNode> OriginalPattern = new List<PatternNode>();
    [Serializable]
    public class PatternNode
    {
        [HideInInspector] public string elementTitle = "Node";
        public int xAxis;
        public int yAxis;
        public bool isGood;
    }
    public void UseCard(Unit unitUsingCard,Node n)
    {
        unitUsingCard.DecreaseAPBy(cost);
        foreach(var p in pattern)
        {
            Node target = n;

            for (int i = 0; i < Mathf.Abs(p.xAxis); i++)
            {
                if (p.xAxis > 0)
                {
                    target = target?.GetNeighbor(Direction.Right);
                }
                else
                    target = target?.GetNeighbor(Direction.Left);
            }
            for (int j = 0; j < Mathf.Abs(p.yAxis); j++)
            {
                if (p.yAxis > 0)
                {
                    target = target?.GetNeighbor(Direction.Up);
                }
                else
                    target = target?.GetNeighbor(Direction.Down);
            }
            if (!target)
                continue;
            if(target.occupyingUnit!=null)
            {
                target.occupyingUnit.Damage((p.isGood)?-effectIntensity:effectIntensity);
            }
        }

    }

}