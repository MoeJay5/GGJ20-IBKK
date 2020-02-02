using UnityEngine;

public class WorldspaceCanvas_FaceCamera : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = LevelStateManager.Instance.gameCamera;
    }

    private void Update()
    {
        transform.LookAt(LevelStateManager.Instance.gameCamera.transform);
        transform.right = -transform.right;
    }
}
