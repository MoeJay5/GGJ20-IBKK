using UnityEngine;

public class WorldspaceCanvas_FaceCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(LevelStateManager.Instance.gameCamera.transform);
        transform.right = -transform.right;
    }
}
