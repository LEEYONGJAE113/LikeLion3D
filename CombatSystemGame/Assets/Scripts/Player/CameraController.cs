using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed; // default 2
    [SerializeField] float distanceFromTarget; // default 5
    [SerializeField] float minVerticalAngle; // default -20
    [SerializeField] float maxVerticalAngle; // default 45
    [SerializeField] Vector2 framingOffset; // default (0, 1), 카메라 위치 오프셋

    [SerializeField] bool invertHorizontal; // 마우스 회전 반전
    [SerializeField] bool invertVertical;

    float rotationX;
    float rotationY;

    float invertHorizontalValue;
    float invertVerticalValue;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        invertHorizontalValue = invertHorizontal ? -1 : 1;
        invertVerticalValue = invertVertical ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * invertVerticalValue;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * invertHorizontalValue;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);

        // var desiredCameraPos = focusPosition - targetRotation * new Vector3(0, 0, distanceFromTarget); // 버그수정용

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distanceFromTarget);
        // transform.position = desiredCameraPos;
        transform.rotation = targetRotation;
    }

    public Quaternion PlayerRotation => Quaternion.Euler(0, rotationY, 0);
}
