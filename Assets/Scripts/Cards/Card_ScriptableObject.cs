using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class Card_ScriptableObject : ScriptableObject
{
    public string cardName;
    public int cost;
    public int effectIntensity;
    public Texture2D cardImage;
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

}