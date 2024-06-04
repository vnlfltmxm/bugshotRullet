using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Mirror;

public class LocalPlayer : NetworkBehaviour
{
    [SerializeField]
    private Camera localPlayer_Camera;

    private float _mouseXRotate;
    private float _mouseYRotate;
    private float _cameraXRotate;
    private float _cameraYRotate;
    private float _rotateSpeed = 150.0f;


    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        //if(Input.mousePosition.x < Screen.width 
        //    && Input.mousePosition.y < Screen.height)
        //{
        //    CameraRotate();
        //}
        CameraRotate();
        MouseTest();
    }

    private void CameraRotate()
    {
        _mouseYRotate = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        _mouseXRotate = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _cameraXRotate += _mouseXRotate;
        _cameraYRotate += _mouseYRotate;

        _cameraYRotate = Mathf.Clamp(_cameraYRotate, -10, 20);
        _cameraXRotate = Mathf.Clamp(_cameraXRotate, -60, 60);
        localPlayer_Camera.transform.rotation = Quaternion.Euler(_cameraYRotate, _cameraXRotate, 0f);
        //localPlayer_Camera.transform.eulerAngles = new Vector3(_cameraYRotate, _cameraXRotate, 0);
    }

    private void MouseTest()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
