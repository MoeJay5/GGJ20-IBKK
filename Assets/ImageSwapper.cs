using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageSwapper : MonoBehaviour
{
    public List<Sprite> cycles = new List<Sprite>();
    public Image source;
    float timer = .75f;
    private int index=0;
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(SwapImages());
    }

    IEnumerator SwapImages()
    {
        while (true)
        {
            source.sprite = cycles[index];
            yield return new WaitForSeconds(timer);
            index++;
            if (index >= cycles.Count)
                index = 0;
        }
    }

}
