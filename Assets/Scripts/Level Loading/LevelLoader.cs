using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    public Slider loadingBar;
    public static LevelLoader instance;
    private static string levelToLoad;
    private void Awake()
    {
        instance = this;
        StartCoroutine(LoadScene(levelToLoad));
    }
    public static void LoadLevelByNumber(int levelNumber)
    {
        LoadSceneByName("Level" + levelNumber.ToString());
    }


    [ContextMenu("LoadMainMenu")]
    public static void LoadMainMenu()
    {
        LevelLoader.levelToLoad = "TitleScene";
        SceneManager.LoadScene("LoadingScreen");
        
        //instance.StartCoroutine(instance.LoadScene("TitleScene"));
    }
    public static void LoadSceneByName(string name)
    {
        LevelLoader.levelToLoad = name;
        SceneManager.LoadScene("LoadingScreen");
        //instance.StartCoroutine(instance.LoadScene(name));
    }

   

    public IEnumerator LoadScene(string name)
    {
        yield return null;
        loadingBar = GameObject.FindObjectOfType<Slider>();
        float timer = 0;
        var asyncOp = SceneManager.LoadSceneAsync(name);
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress<.89f)
        {
            timer += Time.deltaTime;
            
            loadingBar.value = asyncOp.progress / 2f;
            yield return null;
        }
        if(timer<5)
        {
            while(loadingBar.value <1)
            {
                loadingBar.value += Random.Range(.000025f, .002f);
                yield return null;
            }
        }
        else
        {
            float waitASec = 1;
            while(waitASec>0)
            {
                waitASec -= Time.deltaTime;
                loadingBar.value += Time.deltaTime / 2f;
            }
        }
        //while(timer < 5)
        //{
        //    timer += Time.deltaTime;
        //
        //    yield return null;
        //
        //}
        asyncOp.allowSceneActivation = true;
        Destroy(this);
    }


}
