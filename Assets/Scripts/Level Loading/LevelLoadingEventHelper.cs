using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadingEventHelper : MonoBehaviour
{
   public void LoadMainMenu()
    {
        LevelLoader.LoadMainMenu();
    }
    public void LoadLevelByNumber(int number)
    {
        LevelLoader.LoadLevelByNumber(number);
    }
    public void LoadLevelByName(string name)
    {
        LevelLoader.LoadSceneByName(name);
    }
}
