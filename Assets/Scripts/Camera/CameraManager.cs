using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header ("Scene References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform lookAtTarget;

    [Header ("Movement & Rotation Properties")]
    [SerializeField] private float boundDistance = 2f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float movementFallOffSpeed = 2f;
    [SerializeField] private float rotationFallOffSpeed = 1f;

    [Header ("Zoom Properties")]
    [SerializeField] private float minFov = 45f;
    [SerializeField] private float maxFov = 100f;
    [SerializeField] private float sensitivity = 10f;

    private float movement_FallOff;
    private float rotation_FallOff;
    private Vector3 initializedPosition;
    private Vector3 newPosition;
    private Vector3 newRotation;

    private void OnEnable ()
    {
        initializedPosition = mainCamera.transform.localPosition;
    }

    void Update ()
    {
        CameraZoom ();

        MovementRotation ();

        FallOff ();
    }

    private void CameraZoom ()
    {
        //Zoom functionality
        float fov = mainCamera.fieldOfView;
        fov -= Input.mouseScrollDelta.y * sensitivity;
        fov = Mathf.Clamp (fov, minFov, maxFov);
        mainCamera.fieldOfView = fov;
    }

    private void MovementRotation ()
    {
        //Positioning functionality
        if (Input.GetMouseButton (0) && rotation_FallOff == 0)
        {
            float speed = this.movementSpeed * Time.deltaTime;
            newPosition = new Vector3 (InputListener.Instance.Mouse_X * speed, 0, 0);
            movement_FallOff = 1f;
        }
        //Rotation functionality
        else if (Input.GetMouseButton (1) && movement_FallOff == 0)
        {
            float speed = this.movementSpeed * Time.deltaTime;
            newRotation = new Vector3 (0, InputListener.Instance.Mouse_X * speed, 0);
            rotation_FallOff = 1f;
        }
    }

    private void FallOff ()
    {
        if (movement_FallOff > 0)
        {
            movement_FallOff -= Time.deltaTime * movementFallOffSpeed;

            if ((-newPosition + mainCamera.transform.localPosition - initializedPosition).sqrMagnitude <= boundDistance)
                mainCamera.transform.Translate (-newPosition * movement_FallOff);
        }
        else
            movement_FallOff = 0;

        if (rotation_FallOff > 0)
        {
            rotation_FallOff -= Time.deltaTime * rotationFallOffSpeed;
            transform.RotateAround (lookAtTarget.position, newRotation, rotation_FallOff * 2);
        }
        else
            rotation_FallOff = 0;
    }
}