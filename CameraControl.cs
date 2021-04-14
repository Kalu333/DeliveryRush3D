using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    [Range(0, 5f)]
    private float smoothness = 0.1f;

    private Camera main_camera = null;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        main_camera = GetComponent<Camera>();
        EventHandler.ChangeCameraHeight = (value) =>
        {
            offset = new Vector3(offset.x, value, offset.z);
            update_position(0);
        };
        EventHandler.ChangeCameraForward = (value) => 
        {
            offset = new Vector3(offset.x, offset.y, value);
            update_position(0);
        };
        EventHandler.ChangeCameraRotation = (value) => transform.eulerAngles = new Vector3(value, transform.rotation.y, transform.rotation.z);
        EventHandler.SetCameraFov = (value) => main_camera.fieldOfView = value;
        EventHandler.GetCameraFov = () => main_camera.fieldOfView;
        EventHandler.GetCameraOffset = () => offset;
        EventHandler.GetCameraRotation = () => transform.eulerAngles.x;
        EventHandler.SetCameraSmoothness = (value) => smoothness = value;
        EventHandler.GetCameraSmoothness = () => smoothness;
    }

    private void Start()
    {
        target = EventHandler.GetPlayerTransform.Invoke();

        if (null == target)
            Debug.LogError("Player transform not attached to camera!");
        else
            calculate_offset();

        //EventHandler.ChangeCameraHeight += (value) => calculate_offset();
        //EventHandler.ChangeCameraForward += (value) => calculate_offset();
        //EventHandler.ChangeCameraRotation += (value) => calculate_offset();
        EventHandler.SetPlayerPosition += (value) => update_position(0);
    }

    private void LateUpdate()
    {
        if (null != target)
        {
            update_position(smoothness);
        }
        else
        {
            target = EventHandler.GetPlayerTransform.Invoke();
            if (null != target)
                calculate_offset();
        }
    }

    private void update_position(float _smoothness)
    {
        Vector3 target_position = target.position + offset;
        if (0 == _smoothness)
        {
            transform.position = target_position;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, target_position, ref velocity, _smoothness);
        }
        //transform.LookAt(transform);
    }

    private void calculate_offset()
    {
        offset = transform.position - target.position;
    }
}
