using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float startSize = 6f;

    [SerializeField]
    private float minSize = 2f;

    [SerializeField]
    private float wideDividerCoeff = 2f;

    [SerializeField]
    private float sizeCoeff = 0.1f;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Vector3 offset;

    public void UpdateCameraSizeAndPosition(Vector3 center, Vector2 size)
    {
        var cameraPosition = transform.position;
        cameraPosition.x = center.x;
        cameraPosition.z = center.z;

        transform.position = cameraPosition + offset;

        var nextSize = startSize;

        if (size.x > size.y)
        {
            // too wide
            if (size.x / size.y > 2f)
            {
                nextSize = Mathf.Max(size.x, size.y) / wideDividerCoeff;
            }
            else
            {
                nextSize = Mathf.Min(size.x, size.y);
            }
        }
        else
        {
            nextSize = Mathf.Max(size.x, size.y);
        }

        nextSize *= sizeCoeff * startSize;

        camera.orthographicSize = Mathf.Max(nextSize, minSize);
    }
}
