using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FollowTranaform : MonoBehaviour
{
    public Transform transformToFollow;
    public Vector3 offsetFromTransform;

    [Range(0.1f, 1.0f)]
    public float _smoothFactor;

    public void SetFollowTransform(Transform followTransform)
    {
        transformToFollow = followTransform;
        if (transformToFollow)
        {
            offsetFromTransform = transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void LateUpdate()
    {
        if (!transformToFollow)
        {
            return;
        }
        Vector3 newPosition = transformToFollow.position + offsetFromTransform;
        transform.position = Vector3.Slerp(transform.position, newPosition, _smoothFactor);
        transform.LookAt(transformToFollow);
    }
}
