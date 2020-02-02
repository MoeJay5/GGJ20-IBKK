using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DEBUGSKIPLEVEL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F1)&&Input.GetKey(KeyCode.Alpha1))
        {
            var scene =SceneManager.GetActiveScene();
            switch (scene.name)
            {
                case "Level 1":
                    {
                        LevelLoader.LoadSceneByName("Level 2");

                        break;
                    }
                case "Level 2":
                    {
                        LevelLoader.LoadSceneByName("Level 3");

                        break;
                    }
                case "Level 3":
                    {
                        LevelLoader.LoadSceneByName("Level 4");

                        break;
                    }
                case "Level 4":
                    {
                        LevelLoader.LoadSceneByName("Level 5");

                        break;
                    }

            }

        }
    }
}
