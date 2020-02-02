using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardUtility
{
   
    public static List<Card_ScriptableObject.PatternNode> RotatePattern(List<Card_ScriptableObject.PatternNode> toRotate,float angleDegrees)
    {
        List<Card_ScriptableObject.PatternNode> newPattern = new List<Card_ScriptableObject.PatternNode>();

        for (int i = 0; i <toRotate.Count; i++)
        {
            Card_ScriptableObject.PatternNode pattern = new Card_ScriptableObject.PatternNode();
            var ogPatern = toRotate[i];
            pattern.xAxis = (int)((ogPatern.xAxis * Mathf.Cos(angleDegrees * Mathf.Deg2Rad)) - (ogPatern.yAxis * Mathf.Sin(angleDegrees * Mathf.Deg2Rad)));
            pattern.yAxis = (int)((ogPatern.xAxis * Mathf.Sin(angleDegrees * Mathf.Deg2Rad)) + (ogPatern.yAxis * Mathf.Cos(angleDegrees * Mathf.Deg2Rad)));
            pattern.elementTitle = ogPatern.elementTitle;
            pattern.isGood = ogPatern.isGood;
            newPattern.Add(pattern);
        }
        return newPattern;
    }


}
