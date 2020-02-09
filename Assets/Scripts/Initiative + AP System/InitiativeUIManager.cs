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

    private class InternalNodeState
    {
        public Unit unit;
        public Image image;
        public float start;
        public float target;

        public InternalNodeState(Unit u, Image i, float s, float t)
        {
            unit = u;
            image = i;
            start = s;
            target = t;
        }
    }

    private List<InternalNodeState> currentProcessingWindow = new List<InternalNodeState>();
    
    // Start is called before the first frame update
    void Start()
    {
        InitiativeSystem.registerManager(this);
        InitiativeSystem.nextTurn();
    }

    void Update()
    {
        if(InputListener.Instance.CheatNextTurn) InitiativeSystem.nextTurn();
    }

    public void TriggerInitiativeChange()
    {
        List<Unit> currentQueue = InitiativeSystem.currentQueue.ToList();

        if (currentQueue.Count == 0)
        {
            InitiativeSystem.finishNextTurn();
            return;
        }
        
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
            newIcon.sprite = val.InGamePlay ? val.unitIcon : val.unitIconDisabled;
            newIcon.transform.localPosition = Vector3.right * 900f;
            existingImages.Add(val, newIcon);
        });
        
        currentProcessingWindow.Clear();
        
        for (int index = 0; index < currentQueue.Count; index++)
        {
            Unit u = currentQueue[index];
            Image i = existingImages[u];
            // Update the images.
            i.sprite = u.InGamePlay ? u.unitIcon : u.unitIconDisabled;
            float t = (1800f / currentQueue.Count) * index - 900f;
            currentProcessingWindow.Add(new InternalNodeState(u, i, i.transform.localPosition.x, t));
        }
        
        StartCoroutine(transitionTurns());
    }

    IEnumerator transitionTurns()
    {
        float startTime = Time.time;

        while (Time.time - startTime < 1f)
        {
            float totalTime = Time.time - startTime;
            currentProcessingWindow.ForEach(state =>
            {
                state.image.transform.localPosition =
                    Vector3.right * Mathf.Lerp(state.start, state.target, totalTime);
            });

            yield return null;
        }
        
        currentProcessingWindow.ForEach(state =>
        {
            state.image.transform.localPosition = Vector3.right * state.target;
        });
        InitiativeSystem.finishNextTurn();
    }
}
