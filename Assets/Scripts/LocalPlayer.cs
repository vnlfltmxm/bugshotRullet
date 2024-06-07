using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Asn1.Cmp;

public class LocalPlayer : NetworkBehaviour
{
    [HideInInspector]
    public bool _isLocalPlayerTurn = false;

    public Transform _gunPos;

    public Camera localPlayer_Camera;

    private float _mouseXRotate;
    private float _mouseYRotate;
    private float _cameraXRotate;
    private float _cameraYRotate;
    private float _rotateSpeed = 150.0f;

    public TextMesh _textMesh;
    [SyncVar]
    private string _text;

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
        _textMesh.text = netId.ToString();
        if (!isLocalPlayer)
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
        //ShoutGunTest();
        
        if (_isLocalPlayerTurn)
        {
            if (Vector3.Distance(this._gunPos.position, GameManger.Instance._gun.transform.position) >= 10.0f)
                MoveGunToPlayer(this.gameObject);

            AimingShoutGun();
            ReadyShoutGun();
        }
    }
    [Command]
    private void MoveGunToPlayer(GameObject player)
    {
        ShoutGun.Instance.MoveToPlayer(player);
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

    private void AimingShoutGun()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AimingSelf(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AimingForward(this.gameObject);
        }
    }

    [Command]
    private void Fire()
    {
        ShoutGun.Instance.FireShoutGun();
    }

    private void ReadyShoutGun()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this._isLocalPlayerTurn = false;
            Fire();
        }
            
    }


    [Command]
    private void RegistrationSelf(GameObject player)
    {
        GameManger.Instance.SetPlayers(this.gameObject);
    }
    [Command]
    private void AimingSelf(GameObject player)
    {
        ShoutGunRotate(player);
    }
    [Command]
    private void AimingForward(GameObject player)
    {
        ShoutGunForward(player);
    }
    [Server]
    private void ShoutGunForward(GameObject player)
    {
        GameManger.Instance._gun.GetComponent<ShoutGun>().ShoutGunAimingForward(player);
    }
    [Server]
    private void ShoutGunRotate(GameObject player)
    {
        GameManger.Instance._gun.GetComponent<ShoutGun>().ShoutGunAimingSelf(player);
    }
}
