using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraFitter : MonoBehaviour
{
    private Camera camera;

    public float defaultCoeff = 9f / 16f;
    public float defaultSize = 60f;

    public float sizeChangeMultiplier = 1.2f;

    private Vector2 resolution;

    private Transform cameraTransform;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        resolution = new Vector2(Screen.width, Screen.height);

        cameraTransform = camera.transform;

        FitCamera();

        StartCoroutine(CheckScreenChanged());
    }

    private void FitCamera()
    {
        var screenCoeff = Screen.width / (float) Screen.height;

        if (screenCoeff < defaultCoeff)
        {
            var diff = 1f + (defaultCoeff - screenCoeff);

            camera.fieldOfView = defaultSize * diff * sizeChangeMultiplier;
        }
        else
        {
            camera.fieldOfView = defaultSize;
        }
    }

    private IEnumerator CheckScreenChanged()
    {
        while (true)
        {
#if UNITY_EDITOR
            FitCamera();
            yield return null;
#else
            yield return new WaitForSeconds(1f);
#endif

            if (Mathf.Abs(resolution.x - Screen.width) > 0.0001f || Mathf.Abs(resolution.y - Screen.height) > 0.0001f)
            {
                resolution.x = Screen.width;
                resolution.y = Screen.height;

                FitCamera();
            }
        }
    }
}
