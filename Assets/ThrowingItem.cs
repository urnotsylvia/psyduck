using UnityEngine;

public class ThrowingItem : MonoBehaviour
{
    // The maximum number of points in the curve line
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;

    [SerializeField]
    private LineRenderer LineRenderer;
    [SerializeField]
    private Transform ReleasePosition;

    [SerializeField]
    private float Strength = 4.25f;
    
    [SerializeField]
    private Camera mainCamera;
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            LineRenderer.enabled = true;
        }
        else {
            LineRenderer.enabled = false;
        }
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = ReleasePosition.position;
        Transform cameraTransform = mainCamera.transform;
        Vector3 startVelocity = Strength * (cameraTransform.forward + cameraTransform.up * 1.5f);
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (-9.8f / 2f * time * time);

            LineRenderer.SetPosition(i, point);

        }


    }
}


