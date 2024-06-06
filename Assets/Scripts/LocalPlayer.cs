using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Mirror;

public class LocalPlayer : NetworkBehaviour
{
    [HideInInspector]
    public bool _isLocalPlayerTurn;

    public Transform _gunPos;

    [SerializeField]
    private Camera localPlayer_Camera;

    private float _mouseXRotate;
    private float _mouseYRotate;
    private float _cameraXRotate;
    private float _cameraYRotate;
    private float _rotateSpeed = 150.0f;



    private void Start()
    {
        if (!isLocalPlayer)
        {
            localPlayer_Camera.gameObject.SetActive(false);
            return;
        }
        localPlayer_Camera.transform.forward = transform.forward;        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        RegistrationSelf(this.gameObject);
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

        if (!_isLocalPlayerTurn)
        {
            return;
        }
        if (_gunPos != GameManger.Instance._gun.transform)
            MoveGunToPlayer(this.gameObject);
    }
    [Command]
    private void MoveGunToPlayer(GameObject player)
    {
        GameManger.Instance._gun.GetComponent<ShoutGun>().MoveToPlayer(player);
        //MoveGun(player);
    }
    private void CameraRotate()
    {
        _mouseYRotate = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        _mouseXRotate = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _cameraXRotate += _mouseXRotate;
        _cameraYRotate += _mouseYRotate;

        _cameraYRotate = Mathf.Clamp(_cameraYRotate, -10, 20);
        _cameraXRotate = Mathf.Clamp(_cameraXRotate, -60, 60);
        localPlayer_Camera.transform.localRotation = Quaternion.Euler(_cameraYRotate, _cameraXRotate, 0f);
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

    [Command]
    private void RegistrationSelf(GameObject player)
    {
        GameManger.Instance.SetPlayers(this.gameObject);
    }

}
