using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if(target == null)
        {
            if (Player.LocalPlayer == null)
                return;

            target = Player.LocalPlayer.transform;
            offset = transform.position - target.position;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
