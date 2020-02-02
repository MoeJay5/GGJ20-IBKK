using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeUIManager : MonoBehaviour
{
    [SerializeField] private Image InitiativeBar;
    [SerializeField] private Image IconPrefab;

    private Dictionary<Unit, Image> existingImages = new Dictionary<Unit, Image>();

    // Start is called before the first frame update
    void Start()
    {
        InitiativeSystem.registerManager(this);
        InitiativeSystem.nextTurn();
    }

    IEnumerator transitionTurns()
    {
        List<Unit> currentQueue = InitiativeSystem.currentQueue.ToList();

        if (currentQueue.Count == 0) yield break;
        
        currentQueue.Add(currentQueue[0]);
        currentQueue.RemoveAt(0);

        // Clear old images
        existingImages.Keys.Where(key => !currentQueue.Contains(key)).ToList().ForEach(val =>
        {
            Destroy(existingImages[val]);
            existingImages.Remove(val);
        });
        
        // Initiate images for new queue items
        currentQueue.Where(val => !existingImages.ContainsKey(val)).ToList().ForEach(val =>
        {
            Image newIcon = Instantiate(IconPrefab, InitiativeBar.transform);
            newIcon.sprite = val.unitIcon;
            existingImages.Add(val, newIcon);
        });

        for (int index = 0; index < currentQueue.Count; index++)
        {
            Unit u = currentQueue[index];
            Image i = existingImages[u];
            float offset = (1800f / currentQueue.Count) * index - 900;
            i.transform.localPosition = Vector3.left * offset;
        }
        
        yield return new WaitForSeconds(1f);
        
        InitiativeSystem.finishNextTurn();
    }

    public void TriggerInitiativeChange()
    {
        StartCoroutine(transitionTurns());
    }
}
