using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBillboarding : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        // Get the main camera's transform
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (camTransform != null)
        {
            // Make the text face the same direction as the camera
            transform.rotation = camTransform.rotation;
        }
    }
}
